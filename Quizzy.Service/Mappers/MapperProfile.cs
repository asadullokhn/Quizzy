using AutoMapper;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Answer;
using Quizzy.Service.DTOs.Attempt;
using Quizzy.Service.DTOs.Question;
using Quizzy.Service.DTOs.Quiz;
using Quizzy.Service.DTOs.User;

namespace Quizzy.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserCreationDto, User>();
        CreateMap<User, UserViewDto>();

        CreateMap<QuizCreationDto, Quiz>();
        CreateMap<Quiz, QuizViewDto>();

        CreateMap<QuestionCreationDto, Question>();
        CreateMap<Question, QuestionViewDto>();

        CreateMap<AnswerCreationDto, Answer>();
        CreateMap<Answer, AnswerViewDto>();

        CreateMap<AttemptCreationDto, Attempt>();
    }
}