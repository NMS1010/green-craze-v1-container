using FluentValidation;
using green_craze_be_v1.Application.Model.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.ProductImage
{
    public class UpdateProductImageRequestValidator : AbstractValidator<UpdateProductImageRequest>
    {
        public UpdateProductImageRequestValidator()
        {
            RuleFor(x => x.Image).NotEmpty().NotNull();
        }
    }
}