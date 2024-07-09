namespace BookstoreAPI.Models;

public class BookGenerallyInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? AuthorId { get; set; }
    public int AvailableQuantity { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }

    public BookGenerallyInfo()
    {
    }
}