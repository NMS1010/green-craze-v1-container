using FluentValidation;
using green_craze_be_v1.Application.Model.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Address
{
    public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
    {
        public UpdateAddressRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.Receiver).NotEmpty().NotNull();
            RuleFor(x => x.Street).NotEmpty().NotNull();
            RuleFor(x => x.ProvinceId).NotEmpty().NotNull();
            RuleFor(x => x.DistrictId).NotEmpty().NotNull();
            RuleFor(x => x.WardId).NotEmpty().NotNull();
        }
    }
}