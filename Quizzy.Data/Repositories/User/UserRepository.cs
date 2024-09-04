using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class UserRepository(QuizzyDbContext dbContext) : IUserRepository
{
    public User Update(User entity)
        => dbContext.Users.Update(entity).Entity;

    public async Task<User> CreateAsync(User entity)
        => (await dbContext.Users.AddAsync(entity)).Entity;

    public Task<User?> GetByEmailAsync(string email)
        => dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    public Task<User?> GetByIdAsync(int id) => dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

    public IQueryable<User> Where(Expression<Func<User, bool>>? expression = null)
        => expression is null ? dbContext.Users : dbContext.Users.Where(expression);

    public async Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
    {
        var entity = await dbContext.Users.FirstOrDefaultAsync(expression);

        if (entity is null)
            return false;

        dbContext.Users.Remove(entity);

        return true;
    }

    public Task RemoveRangeAsync(IEnumerable<User> users)
    {
        dbContext.Users.RemoveRange(users);
        return Task.CompletedTask;
    }

    public Task<bool> AnyAsync(string email)
        => dbContext.Users.AnyAsync(u => u.Email.Equals(email));

    public Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        => dbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password == password);
}