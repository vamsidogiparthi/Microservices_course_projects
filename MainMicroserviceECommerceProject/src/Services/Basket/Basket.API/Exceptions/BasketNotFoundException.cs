

namespace Basket.API.Exceptions;

public class BasketNotFoundException(string userName) : NotFoundException("basket", userName)
{
}
