namespace BookstoreAPI
{
    public class BookDTO
    {
        public int Id { get; set; }
        public int NumberPages { get; set; }
        public string Language { get; set; }
        public DateTime YearPublication { get; set; }
        public string Description { get; set; }
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int AvailableQuantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int Likes { get; set; }
        public string? PhotoUrl { get; set; }



        public BookDTO()
        {
        }

        public override string ToString()
        {
            return @$"{nameof(Id)}: {Id}, {nameof(NumberPages)}: {NumberPages}, {nameof(Language)}: {Language}, 
                {nameof(YearPublication)}: {YearPublication}, {nameof(Description)}: {Description}, 
                {nameof(PublisherId)}: {PublisherId}, {nameof(Name)}: {Name}, {nameof(AuthorId)}: {AuthorId}, 
                {nameof(AvailableQuantity)}: {AvailableQuantity}, {nameof(Price)}: {Price}, {nameof(Discount)}: {Discount}, 
                {nameof(Likes)}: {Likes}, {nameof(PhotoUrl)}: {PhotoUrl}";
        }
    }
}
