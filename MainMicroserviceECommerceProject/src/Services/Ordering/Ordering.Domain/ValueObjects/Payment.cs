﻿
namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string CVV { get; } = default!;
    public string Expiration { get; } = default!;
    public int PaymentMethod { get; } = default!;

    protected Payment()
    {

    }
    private Payment(string cardName, string cardNumber, string cvv, string expiration, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        CVV = cvv;
        Expiration = expiration;
        PaymentMethod = paymentMethod;

    }

    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
        
        
        return new Payment(cardName, cardNumber, cvv, expiration, paymentMethod);
    }
}
