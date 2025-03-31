using System.ComponentModel.DataAnnotations;

namespace Budget_Management.Validaciones
{
    public class IsFirstLetterCapitalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(value.ToString()) || value == null)
            {
                return ValidationResult.Success;
            }
            if (char.IsLower(value.ToString()[0]))
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}
