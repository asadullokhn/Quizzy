using Microsoft.AspNetCore.Http;

namespace Quizzy.Service.Helpers;

public class HttpContextHelper
{
    public static IHttpContextAccessor Accessor;
    public static HttpContext Context => Accessor.HttpContext ?? new DefaultHttpContext();
    public static IHeaderDictionary ResponseHeaders => Context.Response?.Headers ?? new HeaderDictionary();

    public static int UserId => int.Parse(Context?.User?.FindFirst("Id")?.Value ?? "-1");
    public static string? Role => Context?.User?.FindFirst("Role")?.Value;

}