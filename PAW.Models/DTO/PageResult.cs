namespace PAW.Models.DTO;
public class PageResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}
public record LookupOption(string Id, string Name);
