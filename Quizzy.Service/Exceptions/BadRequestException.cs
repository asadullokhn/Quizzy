using System.Net;

namespace Quizzy.Service.Exceptions;

public class BadRequestException(string message) : StatusCodeException(HttpStatusCode.BadRequest, message) { }