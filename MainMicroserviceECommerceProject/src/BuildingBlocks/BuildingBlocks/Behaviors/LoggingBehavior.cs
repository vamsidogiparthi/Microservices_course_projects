using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResonse>(ILogger<LoggingBehavior<TRequest, TResonse>> logger) : IPipelineBehavior<TRequest, TResonse>
    where TRequest : notnull, IRequest<TResonse>
    where TResonse : notnull
{
    public async Task<TResonse> Handle(TRequest request, RequestHandlerDelegate<TResonse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Start] HandleRequest = {Request} - Response = {Response}", typeof(TRequest).Name, typeof(TResonse).Name);

        var response = await next(cancellationToken);
        logger.LogInformation("[End] HandledRequest = {Request} - Response = {Response}", typeof(TRequest).Name, typeof(TResonse).Name);

        return response;
    }
}
