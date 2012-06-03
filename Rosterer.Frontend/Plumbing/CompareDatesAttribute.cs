using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rosterer.Frontend.Plumbing
{
    [System.AttributeUsage(System.AttributeTargets.All,AllowMultiple = false, Inherited = true)]
    public class CompareDatesAttribute : ValidationAttribute
    {

        private static string _controlToCompareto;

        public CompareDatesAttribute(string controlToCompareTo)
        {
            _controlToCompareto = controlToCompareTo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateToCompare = validationContext.ObjectType.GetProperty(_controlToCompareto);
            var dateToCompareValue = dateToCompare.GetValue(validationContext.ObjectInstance, null);
            if (dateToCompareValue != null && value != null && (DateTime)value < (DateTime)dateToCompareValue)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        
    }
}