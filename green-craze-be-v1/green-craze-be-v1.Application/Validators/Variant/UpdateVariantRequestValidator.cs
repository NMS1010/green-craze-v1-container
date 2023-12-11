using FluentValidation;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Variant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Variant
{
    public class UpdateVariantRequestValidator : AbstractValidator<UpdateVariantRequest>
    {
        public UpdateVariantRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Sku).NotEmpty().NotNull();
            RuleFor(x => x.Quantity).NotEmpty().NotNull();
            RuleFor(x => x.ItemPrice).NotEmpty().NotNull();
            RuleFor(x => x.TotalPrice).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
            RuleFor(x => x.Status)
                .Must(x => VARIANT_STATUS.Status.Contains(x))
                .WithMessage("Unexpected variant status");
        }
    }
}