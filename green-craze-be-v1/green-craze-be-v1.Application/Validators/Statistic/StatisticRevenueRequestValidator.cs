using FluentValidation;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Model.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Statistic
{
    public class StatisticRevenueRequestValidator : AbstractValidator<StatisticRevenueRequest>
    {
        public StatisticRevenueRequestValidator()
        {
            RuleFor(x => x.Year).NotNull().NotEmpty();
        }
    }
}
