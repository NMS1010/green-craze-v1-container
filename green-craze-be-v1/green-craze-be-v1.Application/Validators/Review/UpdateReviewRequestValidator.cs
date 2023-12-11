using FluentValidation;
using green_craze_be_v1.Application.Model.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Review
{
    internal class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequest>
    {
        public UpdateReviewRequestValidator()
        {
            RuleFor(x => x.Rating).NotEmpty().NotNull();
            RuleFor(x => x.Title).NotEmpty().NotNull();
            RuleFor(x => x.Content).NotEmpty().NotNull();
        }
    }
}