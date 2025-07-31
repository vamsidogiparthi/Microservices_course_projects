using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException(string entityName, object key) : NotFoundException(entityName, key)    
{
}

