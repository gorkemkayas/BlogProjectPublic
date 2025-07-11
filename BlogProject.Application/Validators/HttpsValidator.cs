using System.ComponentModel.DataAnnotations;

namespace BlogProject.Application.Validators
{
    public class HttpsValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if(value == null)
            {
                return ValidationResult.Success;
            }

            string url = value.ToString()!;

            if(!url!.StartsWith("https://",StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult("Social Platform links have to start with 'https://'");
            }

            return ValidationResult.Success;

        }
    }
}
