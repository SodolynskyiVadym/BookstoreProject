namespace BookstoreAPI.Models;

public class BookDetailInfo
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int NumberPages { get; set; }
    public string Language { get; set; }
    public int YearPublication { get; set; }
    public string Description { get; set; }
    public int PublisherId { get; set; }

    public BookDetailInfo(int id, int bookId, int numberPages, string language, int yearPublication, string description,
        int publisherId)
    {
        Id = id;
        BookId = bookId;
        NumberPages = numberPages;
        Language = language ?? throw new ArgumentNullException(nameof(language));
        YearPublication = yearPublication;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        PublisherId = publisherId;
    }

    public BookDetailInfo()
    {
    }
}