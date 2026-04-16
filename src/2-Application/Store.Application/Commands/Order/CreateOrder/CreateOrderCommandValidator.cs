using FluentValidation;

namespace Store.Application.Commands.Order.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.NumberOrder)
             .NotEmpty().WithMessage("O número do pedido deve ser informado.")
             .MinimumLength(5).WithMessage("O número do pedido deve ter pelo menos 5 caracteres.")
             .MaximumLength(50).WithMessage("O número do pedido não pode ultrapassar 50 caracteres.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0).WithMessage("O preço total do pedido deve ser maior que zero.");
    }
}
