using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TasksEvaluation.Core.Filters
{
    public class DateLaterThanNowAttribute : ValidationAttribute
    {

        // Set the name of the property to compare
        //public DateGreaterThanAttribute(string comparisonProperty)
        //{
        //    //_comparisonProperty = comparisonProperty;
        //}
        // Validate the date comparison
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            var now = DateTime.Now;
                
                //(DateTime)validationContext
                //.ObjectType.GetProperty(_comparisonProperty).GetValue(validationContext.ObjectInstance);

            if (currentValue < now)
            {
                return new ValidationResult(ErrorMessage = $"Date must be later than now");
            }

            return ValidationResult.Success;
        }
    }
}
