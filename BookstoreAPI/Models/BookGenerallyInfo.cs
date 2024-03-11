namespace BookstoreAPI.Models;

public class BookGenerallyInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? AuthorId { get; set; }
    public int AvailableQuantity { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }
    public int Likes { get; set; }
    public string PhotoUrl { get; set; }

    public BookGenerallyInfo()
    {
    }
}