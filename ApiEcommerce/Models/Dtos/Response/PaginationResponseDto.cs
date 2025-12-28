namespace ApiEcommerce.Models.Dtos.Response;

public class PaginationResponseDto<T>
{
   public int PageNumber { get; set; }
   public int PageSize { get; set; }
   public int TotalPages { get; set; }
   public ICollection<T> Items { get; set; } = new List<T>();
}
