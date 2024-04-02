namespace BookstoreAPI.Models
{
    public class OrderDetailInfo
    {
        public OrderDetailInfo()
        {
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        
    }
}
