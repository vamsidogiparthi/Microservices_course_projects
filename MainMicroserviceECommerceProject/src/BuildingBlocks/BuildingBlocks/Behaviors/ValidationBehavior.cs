using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validatiors) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validatiors.Select(x => x.ValidateAsync(context, cancellationToken)));
        if(validationResults.Any(x=> x.IsValid is false))
        {
            var errors = validationResults.SelectMany(x => x.Errors)
                .Where(x => x is not null)
                .ToList();
            throw new ValidationException(errors);
            
        }
        return await next.Invoke(cancellationToken);
    }
}
