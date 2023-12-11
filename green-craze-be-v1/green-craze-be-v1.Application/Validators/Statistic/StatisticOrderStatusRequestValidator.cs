using FluentValidation;
using green_craze_be_v1.Application.Model.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Statistic
{
    public class StatisticOrderStatusRequestValidator : AbstractValidator<StatisticOrderStatusRequest>
    {
        public StatisticOrderStatusRequestValidator()
        {
            RuleFor(x => x.StartDate).NotEmpty().NotNull();
            RuleFor(x => x.EndDate).NotEmpty().NotNull();
        }
    }
}
