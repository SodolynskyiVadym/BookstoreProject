namespace BookstoreAPI.DTO;

public class CheckoutOrderResponseDTO
{
    public string SessionId { get; set; }
    public string PubKey { get; set; }

    public CheckoutOrderResponseDTO()
    {
    }
}