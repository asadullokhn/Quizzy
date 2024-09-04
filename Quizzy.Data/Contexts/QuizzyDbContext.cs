using Microsoft.EntityFrameworkCore;
using Quizzy.Domain.Entities;

namespace Quizzy.Data.Contexts;

public class QuizzyDbContext(DbContextOptions<QuizzyDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
    public virtual DbSet<QuizQuestions> QuizQuestions { get; set; } = null!;
    public virtual DbSet<Question> Questions { get; set; } = null!;
    public virtual DbSet<Answer> Answers { get; set; } = null!;
    public virtual DbSet<Attempt> Attempts { get; set; } = null!;
    public virtual DbSet<AttemptAnswers> AttemptAnswers { get; set; } = null!;
}