using FluentValidation;
using green_craze_be_v1.Application.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Product
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.BrandId).NotEmpty().NotNull();
            RuleFor(x => x.UnitId).NotEmpty().NotNull();
            RuleFor(x => x.ShortDescription).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Slug).NotEmpty().NotNull();
            RuleFor(x => x.ProductImages).NotEmpty().NotNull();
        }
    }
}