namespace Quizzy.Service.DTOs.Answer;

public class AnswerCreationDto
{
    public required string Content { get; set; }
    public bool IsRight { get; set; }
}