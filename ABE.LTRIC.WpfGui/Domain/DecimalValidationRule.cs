using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ABE.LTRIC.WpfGui.Domain
{
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && decimal.TryParse(value.ToString(), out decimal x))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Field is not a number.");
            }
        }
    }
}
