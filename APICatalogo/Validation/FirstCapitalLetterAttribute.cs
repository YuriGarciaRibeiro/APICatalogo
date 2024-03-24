using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validation;

public class FirstCapitalLetterAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString())){
            return ValidationResult.Success;
        }

        var firstLetter = value.ToString()?.FirstOrDefault().ToString();
        if (firstLetter != firstLetter?.ToUpper())
        {
            return new ValidationResult("A primeira letra do nome da categoria deve ser maiúscula");
        }

        return ValidationResult.Success;



    }



}
