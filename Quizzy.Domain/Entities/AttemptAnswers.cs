using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Domain.Entities;

public class AttemptAnswers
{
    public int Id { get; set; }
    public int AttemptId { get; set; }
    public int AnswerId { get; set; }


    [ForeignKey(nameof(AttemptId))]
    public Attempt? Attempt { get; set; }

    [ForeignKey(nameof(AnswerId))]
    public Answer? Answer { get; set; }
}
