namespace Quizzy.Service.DTOs.Attempt;

public class AttemptViewDto
{
    public int Id { get; set; }
    public IEnumerable<AttemptQuestionViewDto> Questions { get; set; } = [];
    public DateTime StartedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public int DurationInMinutes { get; set; }
}
