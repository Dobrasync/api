using System.ComponentModel.DataAnnotations;

namespace Dobrasync.Api.BusinessLogic.Decorators;

public class RequiredIfAttribute(string propertyName, object desiredValue) : ValidationAttribute
{
    private string PropertyName { get; } = propertyName;
    private object DesiredValue { get; } = desiredValue;

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var type = instance.GetType();
        var property = type.GetProperty(PropertyName);

        if (property == null) return new ValidationResult($"Unknown property: {PropertyName}");

        var propertyValue = property.GetValue(instance, null);

        if (Equals(propertyValue, DesiredValue) && value == null) return new ValidationResult(ErrorMessage);

        return ValidationResult.Success!;
    }
}