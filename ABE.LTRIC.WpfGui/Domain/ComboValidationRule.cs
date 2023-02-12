using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ABE.LTRIC.WpfGui.Domain
{
    public class ComboValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && (int)value != 0)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Field is required.");
            }
        }
    }
}
