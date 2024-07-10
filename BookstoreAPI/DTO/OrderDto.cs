namespace BookstoreAPI.DTO
{
    public class OrderDto
    {
        public Dictionary<int, int> BooksAndQuantity { get; set; }
        public string Destination { get; set; }
        public string PhoneNumber { get; set; }
    }
}
