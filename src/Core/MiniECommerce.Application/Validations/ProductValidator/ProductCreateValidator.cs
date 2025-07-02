using FluentValidation;
using MiniECommerceApp.Application.DTOs.Product;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be positive");
    }
}
