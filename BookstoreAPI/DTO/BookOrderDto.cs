namespace BookstoreAPI.DTO
{
    public class BookOrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int AvailableQuantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
    }
}
