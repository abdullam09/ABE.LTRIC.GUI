﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ABE.LTRIC.WpfGui.Domain
{
    public class DateValidationRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return DateTime.TryParse((value ?? "").ToString(),
                   CultureInfo.CurrentCulture,
                   DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                   out _)
                   ? ValidationResult.ValidResult
                   : new ValidationResult(false, "Invalid date");
        }
    }
}
