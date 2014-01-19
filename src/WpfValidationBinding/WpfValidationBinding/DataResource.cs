using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;

namespace WpfValidationBinding
{
    public class DataResource : Freezable
    {
        public DataResource() { }

        public static readonly DependencyProperty BindingTargetProperty =
            DependencyProperty.Register("BindingTarget", typeof(object), typeof(DataResource),
                new UIPropertyMetadata(null));

        public object BindingTarget
        {
            get { return GetValue(BindingTargetProperty); }
            set { SetValue(BindingTargetProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return Activator.CreateInstance(GetType()) as Freezable;
        }

        protected override void CloneCore(Freezable sourceFreezable)
        {
            base.CloneCore(sourceFreezable);
        }
    }

    public class DataResourceBindingExtension : MarkupExtension
    {
        private object _targetObject;
        private object _targetProperty;
        private DataResource _dataResource;

        public DataResourceBindingExtension() { }

        public DataResource DataResource
        {
            get { return _dataResource; }
            set
            {
                if (_dataResource != value)
                {
                    if (_dataResource != null)
                    {
                        _dataResource.Changed -= DataResource_Changed;
                    }

                    _dataResource = value;

                    if (_dataResource != null)
                    {
                        _dataResource.Changed += DataResource_Changed;
                    }
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            _targetObject = target.TargetObject;
            _targetProperty = target.TargetProperty;

            Debug.Assert(_targetProperty != null || DesignerProperties.GetIsInDesignMode(new DependencyObject()));

            if (DataResource.BindingTarget == null && _targetProperty != null)
            {
                var propertyInfo = _targetProperty as PropertyInfo;

                if (propertyInfo != null)
                {
                    try
                    {
                        return Activator.CreateInstance(propertyInfo.GetType());
                    }
                    catch (MissingMethodException)
                    {
                    }
                }

                var dependencyProperty = _targetProperty as DependencyProperty;

                if (dependencyProperty != null)
                {
                    var obj = _targetObject as DependencyObject;

                    return obj.GetValue(dependencyProperty);
                }
            }

            return DataResource.BindingTarget;
        }

        private void DataResource_Changed(object sender, EventArgs e)
        {
            var dataResource = sender as DataResource;

            var dependencyProperty = _targetProperty as DependencyProperty;

            if (dependencyProperty != null)
            {
                var obj = (DependencyObject)_targetObject;
                var value = Convert(dataResource.BindingTarget, dependencyProperty.PropertyType);

                obj.SetValue(dependencyProperty, value);
                return;
            }

            var propertyInfo = _targetProperty as PropertyInfo;

            if (propertyInfo != null)
            {
                var value = Convert(dataResource.BindingTarget, propertyInfo.PropertyType);

                propertyInfo.SetValue(_targetObject, value, new object[0]);
            }
        }

        private object Convert(Object obj, Type toType)
        {
            try
            {
                return System.Convert.ChangeType(obj, toType);
            }
            catch
            {
                return obj;
            }
        }
    }
}
