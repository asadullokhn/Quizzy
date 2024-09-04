namespace Quizzy.Service.DTOs.User;

public class UserCreationDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}