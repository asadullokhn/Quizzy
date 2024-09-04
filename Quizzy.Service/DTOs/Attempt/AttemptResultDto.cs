namespace Quizzy.Service.DTOs.Attempt;

public class AttemptResultDto
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public double RightAnswers { get; set; }
    public double LeftAnswers { get; set; }
    public double RightAnswersPercentage { get; set; }
    public double LeftAnswersPercentage { get; set; }
}
