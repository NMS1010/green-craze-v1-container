using FluentValidation;
using green_craze_be_v1.Application.Model.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.ProductCategory
{
    public class UpdateProductCategoryRequestValidator : AbstractValidator<UpdateProductCategoryRequest>
    {
        public UpdateProductCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Slug).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}