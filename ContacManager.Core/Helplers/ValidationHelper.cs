
using System.ComponentModel.DataAnnotations;


namespace Services.Helplers
{
    public class ValidationHelper
    {
        internal static void Modulvalidation(object obj) 
        {
            ValidationContext validationcontext = new ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(obj, validationcontext, results, true);
            if (!isValid)
            {
                throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
            }
        }     
    }
}
