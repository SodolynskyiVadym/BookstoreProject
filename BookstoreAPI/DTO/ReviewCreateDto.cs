namespace BookstoreAPI.DTO;

public class ReviewCreateDto
{
    public int BookId { get; set; }
    public string Description { get; set; }
    public int Mark { get; set; }
}