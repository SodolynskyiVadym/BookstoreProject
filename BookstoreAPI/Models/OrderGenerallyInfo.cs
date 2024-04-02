namespace BookstoreAPI.Models
{
    public class OrderGenerallyInfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Destination { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }

        public OrderGenerallyInfo()
        {
        }
    }
}
