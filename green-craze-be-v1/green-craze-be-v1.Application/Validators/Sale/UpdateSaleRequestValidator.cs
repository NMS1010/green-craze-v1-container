using FluentValidation;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Sale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.StartDate).NotEmpty().NotNull();
            RuleFor(x => x.EndDate).NotEmpty().NotNull();
            RuleFor(x => x.PromotionalPercent).NotEmpty().NotNull();
            RuleFor(x => x.Slug).NotEmpty().NotNull();
        }
    }
}