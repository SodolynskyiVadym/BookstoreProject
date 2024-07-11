namespace BookstoreAPI.DTO;

public class ReviewDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Description { get; set; }
    public int Mark { get; set; }
    public string Email { get; set; }
}