﻿using FluentValidation;
using green_craze_be_v1.Application.Model.Brand;

namespace green_craze_be_v1.Application.Validators.Brand
{
    public class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequest>
    {
        public UpdateBrandRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}