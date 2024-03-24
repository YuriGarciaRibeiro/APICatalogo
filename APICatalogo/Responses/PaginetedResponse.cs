namespace APICatalogo.Responses;

public class PaginatedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string? NextPageUrl { get; set; }

    public PaginatedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords, string? nextPageUrl)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        NextPageUrl = nextPageUrl;
    }
}

