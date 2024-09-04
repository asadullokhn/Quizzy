namespace Quizzy.Service.DTOs.Attempt;

public class AttemptAnswerViewDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public bool WasChoosen { get; set; }
}
