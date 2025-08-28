
namespace BuildingBlocks.Pagination;

public class PaginationResult<T>(int pageIndex, int pageSize, long totalCount, IEnumerable<T> items) where T : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long TotalCount { get; } = totalCount;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public IEnumerable<T> Items { get; } = items;
}
