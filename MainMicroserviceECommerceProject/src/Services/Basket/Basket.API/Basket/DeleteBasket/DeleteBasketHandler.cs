
namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketCommandResult>;
public record DeleteBasketCommandResult(bool IsSuccess);
public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name cannot be empty.");            
    }
}
public class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketCommandResult>
{
    public Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        // Here you would typically delete the basket from the database and cache.
        return Task.FromResult(new DeleteBasketCommandResult(true));
    }
}
