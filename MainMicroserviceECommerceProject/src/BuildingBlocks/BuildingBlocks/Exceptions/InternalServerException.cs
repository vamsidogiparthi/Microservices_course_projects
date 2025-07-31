﻿namespace BuildingBlocks.Exceptions;

public class InternalServerException: Exception
{
    public InternalServerException(string message) : base(message)
    {
    }
    public InternalServerException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InternalServerException(string message, string details) 
        : base(message)
    {
        Details = details;
    }

    public string Details { get; set; } = string.Empty;
    public InternalServerException() : base("An internal server error occurred.")
    {
    }
}
