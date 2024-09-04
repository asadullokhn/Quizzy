using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Quizzy.Domain.Commons;
using static Quizzy.Service.Helpers.HttpContextHelper;

namespace Quizzy.Service.Extensions;

public static class CollectionExtensions
{
    public static IEnumerable<T> ToPagedAsEnumerable<T>(this IEnumerable<T> source, PaginationParameters? parameters)
        where T : class
    {
        return Pagination(source.AsQueryable(), parameters);
    }

    public static IQueryable<T> ToPagedAsQueryable<T>(this IQueryable<T> source,
        PaginationParameters? parameters = null)
        where T : class
    {
        return Pagination(source, parameters);
    }

    private static IQueryable<T> Pagination<T>(IQueryable<T> source, PaginationParameters? parameters)
    {
        if (parameters is null || parameters.PageSize < 1 || parameters.PageIndex < 1)
            parameters = new PaginationParameters { PageSize = 20, PageIndex = 1 };

        var paginationMetaData = new PaginationMetaData(source.Count(), parameters.PageSize, parameters.PageIndex);

        if (ResponseHeaders.ContainsKey("X-Pagination"))
            ResponseHeaders.Remove("X-Pagination");

        ResponseHeaders.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetaData));
        ResponseHeaders.Append("Access-Control-Expose-Headers", "X-Pagination");

        return source.Skip(parameters.PageSize * (parameters.PageIndex - 1)).Take(parameters.PageSize);
    }
}