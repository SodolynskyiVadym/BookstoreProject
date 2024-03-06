namespace BookstoreAPI.Models;

public class BookGenerallyInfo
{
    public int BookGenerallyId { get; set; }
    public string Name { get; set; }
    public int? AuthorId { get; set; }
    public int AvailableQuantity { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }
    public int Like { get; set; }
    public string PhotoUrl { get; set; }

    public BookGenerallyInfo(int bookGenerallyId, string name, int? authorId, int availableQuantity, int price,
        int discount, int like, string photoUrl)
    {
        BookGenerallyId = bookGenerallyId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AuthorId = authorId;
        AvailableQuantity = availableQuantity;
        Price = price;
        Discount = discount;
        Like = like;
        PhotoUrl = photoUrl ?? throw new ArgumentNullException(nameof(photoUrl));
    }

    public BookGenerallyInfo()
    {
    }
}