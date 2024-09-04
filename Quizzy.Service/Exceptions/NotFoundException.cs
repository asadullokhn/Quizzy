using System.Net;

namespace Quizzy.Service.Exceptions;

public class NotFoundException(string entityName) : StatusCodeException(HttpStatusCode.NotFound, $"{entityName} not found!") { }
