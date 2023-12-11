using FluentValidation;
using green_craze_be_v1.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.User
{
    public class UpdateStaffRequestValidator : AbstractValidator<UpdateStaffRequest>
    {
        public UpdateStaffRequestValidator()
        {
            RuleFor(x => x.Type).NotEmpty().NotNull();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
            RuleFor(x => x.Dob).NotEmpty().NotNull();
            RuleFor(x => x.FirstName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
        }
    }
}