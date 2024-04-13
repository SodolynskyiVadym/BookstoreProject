namespace BookstoreAPI.DTO;

public class ReviewDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Description { get; set; }
    public int Mark { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
}