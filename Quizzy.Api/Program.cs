using Microsoft.EntityFrameworkCore;
using Quizzy.Api.Extensions;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using Quizzy.Service.Helpers;
using Quizzy.Service.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<QuizzyDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(connectionString, contextOptionsBuilder =>
        contextOptionsBuilder.MigrationsAssembly("Quizzy.Api")));

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("UserPolicy", policy => policy.RequireRole(UserRole.User.ToString(), UserRole.Admin.ToString()))
    .AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRole.Admin.ToString()));

builder.Services.AddCustomServices();
builder.Services.AddCorsService();
builder.Services.AddSwaggerService();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

HttpContextHelper.Accessor = app.Services.GetService<IHttpContextAccessor>() ?? new HttpContextAccessor();

// Custom Middleware
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
