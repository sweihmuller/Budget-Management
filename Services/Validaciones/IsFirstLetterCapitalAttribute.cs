using System.ComponentModel.DataAnnotations;

namespace Budget_Management.Validaciones
{
    public class IsFirstLetterCapitalAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if (char.IsLower(value.ToString()![0]))
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}
