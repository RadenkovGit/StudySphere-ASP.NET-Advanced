namespace StudySphere.Web.Models.Paging;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; }

    public bool HasPreviousPage => this.PageNumber > 1;

    public bool HasNextPage => this.PageNumber < this.TotalPages;
}
