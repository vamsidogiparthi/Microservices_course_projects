

namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(Guid Id): NotFoundException("name", Id)
{
}
