using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WpfValidationBinding
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region プロパティ通知
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region プロパティ

        private int _minValue = -100;

        public int MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged("Minvalue");
                UpdateValidation();
            }
        }

        private int _maxValue = 100;

        public int MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged("MaxValue");
                UpdateValidation();
            }
        }

        private int _defaultValue = 0;

        public int DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                _defaultValue = value;
                OnPropertyChanged("DefaultValue");
            }
        }

        #endregion

        #region Validation更新

        private void UpdateValidation()
        {
            var bindingExpression = TextData.GetBindingExpression(TextBox.TextProperty);

            if (bindingExpression != null)
                bindingExpression.UpdateTarget();
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
