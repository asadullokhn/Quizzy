using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Domain.Entities;

public class Quiz
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }
    public int DurationInMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }
}
