using AutoMapper;
using Quizzy.Data.Repositories;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.User;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Extensions;
using Quizzy.Service.Interfaces;
using System.Linq.Expressions;

namespace Quizzy.Service.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<UserViewDto> UpdateAsync(int id, User user)
    {
        var existUser = await unitOfWork.Users.GetByIdAsync(id)
            ?? throw new NotFoundException("User");

        // Updates goes here
        // user -> existUser
        unitOfWork.Users.Update(existUser);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<UserViewDto>(existUser);
    }

    public async Task<bool> DeleteAsync(Expression<Func<User, bool>> expression)
    {
        var users = unitOfWork.Users.Where(expression);

        if (!users.Any())
            throw new NotFoundException("User");


        await unitOfWork.Users.RemoveRangeAsync(users);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetAsync(int id)
        => await unitOfWork.Users.GetByIdAsync(id) ?? throw new NotFoundException("User");

    public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null,
        PaginationParameters? parameters = null)
    {
        return Task.FromResult<IEnumerable<User>>(unitOfWork.Users.Where(expression).ToPagedAsQueryable(parameters));
    }
}
