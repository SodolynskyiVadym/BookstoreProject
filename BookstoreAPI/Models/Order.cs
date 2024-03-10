namespace BookstoreAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? userId { get; set; }
        public Dictionary<int, int> BooksAndQuantity { get; set; }
        public string Destination { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDelivered { get; set; } = false;

        public Order()
        {
        }
    }
}
