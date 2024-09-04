namespace Quizzy.Service.DTOs.Quiz;

public class QuizCreationDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int DurationInMinutes { get; set; }
}