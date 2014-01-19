using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace WpfValidationBinding
{
    public class IntValidation : ValidationRule
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public IntValidation()
        {
            // 特に設定がない場合はintの最大、最小を使う
            Max = int.MaxValue;
            Min = int.MinValue;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var val = int.Parse(value as string);

                return (Min <= val && val <= Max) ?
                    ValidationResult.ValidResult :
                    new ValidationResult(false, "Input is Out of Range");
            }
            catch
            {
                // intへの変換に例外が発生
                return new ValidationResult(false, "Faild to Parse int");
            }
        }
    }
}
