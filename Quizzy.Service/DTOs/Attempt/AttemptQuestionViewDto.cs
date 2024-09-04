namespace Quizzy.Service.DTOs.Attempt;

public class AttemptQuestionViewDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool WasSubmitted { get; set; }
    public required IEnumerable<AttemptAnswerViewDto> Answers { get; set; }
}