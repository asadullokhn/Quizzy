namespace Quizzy.Domain.Commons;

public class PaginationMetaData(int totalCount, int pageSize, int pageIndex)
{
    public int CurrentPage { get; } = pageIndex;
    public int TotalCount { get; } = totalCount;
    public int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize);

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}