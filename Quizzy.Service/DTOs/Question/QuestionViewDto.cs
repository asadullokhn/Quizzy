using Quizzy.Service.DTOs.Answer;

namespace Quizzy.Service.DTOs.Question;

public class QuestionViewDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required IEnumerable<AnswerViewDto> Answers { get; set; }
}