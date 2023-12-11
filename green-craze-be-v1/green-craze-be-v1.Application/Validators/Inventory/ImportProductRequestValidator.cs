using FluentValidation;
using green_craze_be_v1.Application.Model.Inventory;

namespace green_craze_be_v1.Application.Validators.Inventory
{
    public class ImportProductRequestValidator : AbstractValidator<ImportProductRequest>
    {
        public ImportProductRequestValidator()
        {
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.ActualInventory).NotNull();
            RuleFor(x => x.ProductId).NotNull();
        }
    }
}
