using FluentValidation;
using green_craze_be_v1.Application.Model.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Review
{
    public class ReplyReviewRequestValidator : AbstractValidator<ReplyReviewRequest>
    {
        public ReplyReviewRequestValidator()
        {
            //RuleFor(x => x.Reply).NotEmpty().NotNull();
        }
    }
}