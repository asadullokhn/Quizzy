namespace Quizzy.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int RightAnswersCount { get; set; }
}