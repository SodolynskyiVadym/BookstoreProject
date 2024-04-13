namespace BookstoreAPI.DTO;

public class ReviewCreateDTO
{
    public int BookId { get; set; }
    public string Description { get; set; }
    public int Mark { get; set; }

    public ReviewCreateDTO()
    {
    }
}