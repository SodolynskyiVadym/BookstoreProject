namespace BookstoreAPI.DTO
{
    public class BookCreateUpdateDTO
    {
        public int Id { get; set; }
        public int NumberPages { get; set; }
        public string BookLanguage { get; set; }
        public DateTime YearPublication { get; set; }
        public string Description { get; set; }
        public int PublisherId { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int AvailableQuantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }



        public BookCreateUpdateDTO()
        {
        }
    }
}
