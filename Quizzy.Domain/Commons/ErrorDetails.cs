namespace Quizzy.Domain.Commons;

/// <summary>
/// This class will be used for returning errors
/// </summary>
public class ErrorDetails
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}