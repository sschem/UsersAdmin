using FluentValidation;

namespace UsersAdmin.Core.Model
{
    public class ValidatorBase<T> : AbstractValidator<T>
    {
        protected string CreateNonEmptyMessage(string propertyName) =>
            $"El campo {propertyName} es requerido";

        protected string CreateMaxLengthMessage(string propertyName, int length) =>
            $"El campo {propertyName} debe tener menos de {length} caracteres";
    }
}
