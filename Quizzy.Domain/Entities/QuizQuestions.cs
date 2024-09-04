using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Domain.Entities;

public class QuizQuestions
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public int QuestionId { get; set; }


    [ForeignKey(nameof(QuizId))]
    public Quiz? Quiz { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question? Question { get; set; }
}
