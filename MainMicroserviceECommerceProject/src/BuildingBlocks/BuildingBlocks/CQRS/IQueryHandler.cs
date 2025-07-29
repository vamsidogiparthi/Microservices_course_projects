﻿using MediatR;


namespace BuildingBlocks.CQRS;

public interface IQueryHandler<in TQuery> : IQueryHandler<TQuery, Unit> where TQuery : IQuery<Unit>
{

}
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
}
