namespace ProEventos.Persisttence.Pagination;

public class PageParams
{
    public const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
    }
    public string? Terms { get; set; } = string.Empty;
}
