using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UIDemo.Controls
{
    namespace ValidationRules
    {
        public class NotEmptyRule : ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                string v = (value as string);
                if (v.Length == 0)
                    return new ValidationResult(false, "Field can't be empty");
                return new ValidationResult(true, null);
            }
        }

        public class PositiveDecimalRule : ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                decimal n = 0;
                try
                {
                    string v = (value as string);
                    if (v.Length > 0)
                        n = Decimal.Parse(v);
                }
                catch (Exception)
                {
                    return new ValidationResult(false, "Illegal non-numeric characters");
                }

                if (n <= 0)
                    return new ValidationResult(false, "Illegal non-positive value");

                return new ValidationResult(true, null);
            }
        }

        public class PositiveIntegerRule : ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                int n = 0;
                try
                {
                    string v = (value as string);
                    if (v.Length > 0)
                        n = int.Parse(v);
                }
                catch (Exception)
                {
                    return new ValidationResult(false, "Illegal non-integer characters");
                }

                if (n <= 0)
                    return new ValidationResult(false, "Illegal non-positive value");

                return new ValidationResult(true, null);
            }
        }
    }
}
