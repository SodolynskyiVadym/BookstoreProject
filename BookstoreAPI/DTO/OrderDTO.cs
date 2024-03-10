namespace BookstoreAPI.DTO
{
    public class OrderDTO
    {
        public Dictionary<int, int> BooksAndQuantity { get; set; }
        public string Destination { get; set; }

        public OrderDTO()
        {
        }
    }
}
