using Quizzy.Domain.Entities;

namespace Quizzy.Service.DTOs.User;

public class UserViewDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
}
