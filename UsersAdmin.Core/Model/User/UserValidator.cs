using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsersAdmin.Core.Model.User
{
    public class UserValidator : ValidatorBase<UserDto>
    {
        public UserValidator()
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
