using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Domain.Entities;

public class Answer
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int QuestionId { get; set; }
    public bool IsRight { get; set; }


    [ForeignKey(nameof(QuestionId))]
    public Question? Question { get; set; }
}