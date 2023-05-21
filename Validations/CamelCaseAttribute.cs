using System.ComponentModel.DataAnnotations;

namespace ProyectoPresupuesto.Validations
{
    public class CamelCaseAtrribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()[0].ToString();
            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("El campo debe iniciar con mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}