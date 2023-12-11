using FluentValidation;
using green_craze_be_v1.Application.Model.OrderCancellationReason;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.OrderCancellationReason
{
    public class CreateOrderCancellationReasonRequestValidator : AbstractValidator<CreateOrderCancellationReasonRequest>
    {
        public CreateOrderCancellationReasonRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}