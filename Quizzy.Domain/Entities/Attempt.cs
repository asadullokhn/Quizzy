using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Domain.Entities;

public class Attempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int QuizId { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
    public DateTime ExpireAt { get; set; }


    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [ForeignKey(nameof(QuizId))]
    public Quiz? Quiz { get; set; }
}