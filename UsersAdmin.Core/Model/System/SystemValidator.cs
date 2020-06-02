using FluentValidation;

namespace UsersAdmin.Core.Model.System
{
    public class SystemValidator : ValidatorBase<SystemDto>
    {
        public SystemValidator()
        {
            RuleFor(v => v.Id)
               .NotEmpty().WithMessage(this.CreateNonEmptyMessage("Id"))
               .MaximumLength(20).WithMessage(this.CreateMaxLengthMessage("Id", 20));

            RuleFor(v => v.Name)
               .NotEmpty().WithMessage(this.CreateNonEmptyMessage("Name"))
               .MaximumLength(40).WithMessage(this.CreateMaxLengthMessage("Name", 40));

            RuleFor(v => v.Description)
               .MaximumLength(80).WithMessage(this.CreateMaxLengthMessage("Description", 80));
        }
    }
}
