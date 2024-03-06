namespace BookstoreAPI
{
    public class BookDTO
    {
        public int NumberPages { get; set; }
        public string Language { get; set; }
        public int YearPublication { get; set; }
        public string Description { get; set; }
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public int? AuthorId { get; set; }
        public int AvailableQuantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int Like { get; set; }
        public string PhotoUrl { get; set; }

        public BookDTO(int numberPages, string language, int yearPublication, string description, int publisherId,
            string name, int? authorId, int availableQuantity, int price, int discount, int like, string photoUrl)
        {
            NumberPages = numberPages;
            Language = language ?? throw new ArgumentNullException(nameof(language));
            YearPublication = yearPublication;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            PublisherId = publisherId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AuthorId = authorId;
            AvailableQuantity = availableQuantity;
            Price = price;
            Discount = discount;
            Like = like;
            PhotoUrl = photoUrl ?? throw new ArgumentNullException(nameof(photoUrl));
        }
    }
}
