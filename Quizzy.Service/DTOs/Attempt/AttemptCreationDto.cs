namespace Quizzy.Service.DTOs.Attempt;

public class AttemptCreationDto
{
    public required int QuestionId { get; set; }
    public required int[] Answers { get; set; }
}