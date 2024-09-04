using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quizzy.Data.Repositories;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.User;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quizzy.Service.Services;

public class AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : IAuthService
{
    private readonly string _audience = configuration.GetSection("Jwt:Audience").Value!;
    private readonly double _expire = double.Parse(configuration.GetSection("Jwt:Expire").Value!);
    private readonly string _issuer = configuration.GetSection("Jwt:Issuer").Value!;
    private readonly string _key = configuration.GetSection("Jwt:Key").Value!;

    public async Task<string> LoginAsync(UserLoginDto loginDto)
    {
        var user = await unitOfWork.Users.GetByEmailAndPasswordAsync(loginDto.Email, loginDto.Password)
            ?? throw new NotFoundException("User");

        return CreateToken(user);
    }

    public async Task<UserViewDto> RegisterAsync(UserCreationDto userCreationDto)
    {
        var isUserExist = await unitOfWork.Users.AnyAsync(userCreationDto.Email);
        if (isUserExist) throw new BadRequestException("User already exists");

        var user = mapper.Map<User>(userCreationDto);
        user = await unitOfWork.Users.CreateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<UserViewDto>(user);
    }

    private string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new ClaimsIdentity(new Claim[] {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())});
        //new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(ExtendKeyLengthIfNeeded(securityKey, 256), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(_expire),
            SigningCredentials = credentials,
            Issuer = _issuer, // Add this line
            Audience = _audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static SymmetricSecurityKey? ExtendKeyLengthIfNeeded(SymmetricSecurityKey key, int minLenInBytes)
    {
        if (key != null && key.KeySize < (minLenInBytes * 8))
        {
            var newKey = new byte[minLenInBytes]; // zeros by default
            key.Key.CopyTo(newKey, 0);
            return new SymmetricSecurityKey(newKey);
        }
        return key;
    }
}
