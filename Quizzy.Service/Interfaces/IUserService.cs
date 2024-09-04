using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.User;
using System.Linq.Expressions;

namespace Quizzy.Service.Interfaces;

public interface IUserService
{
    Task<UserViewDto> UpdateAsync(int id, User user);

    Task<bool> DeleteAsync(Expression<Func<User, bool>> expression);

    Task<User> GetAsync(int id);

    Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null,
        PaginationParameters? parameters = null);
}