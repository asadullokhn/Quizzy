using Quizzy.Service.DTOs.Answer;

namespace Quizzy.Service.DTOs.Question;

public class QuestionCreationDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required AnswerCreationDto[] Answers { get; set; }
}