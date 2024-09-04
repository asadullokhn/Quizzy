using System.Net;

namespace Quizzy.Service.Exceptions;

public class StatusCodeException : Exception
{
    public StatusCodeException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public StatusCodeException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = (int)statusCode;
    }

    public int StatusCode { get; }
}