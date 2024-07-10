using BookstoreAPI.DTO;
using BookstoreAPI.Models;
using Stripe.Checkout;

namespace BookstoreAPI.Helpers;

public class StripeHelper
{
    private readonly IConfiguration _config;
    public StripeHelper(IConfiguration config)
    {
        _config = config;
    }
    
    public async Task<string> CheckOut(IEnumerable<BookGenerallyInfo> books, OrderDto order, int userId)
    {
        string? serverUrl = _config.GetSection("Urls:Server").Value;
        string? clientUrl = _config.GetSection("Urls:Client").Value;
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{serverUrl}/order/success/{{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{clientUrl}/clearOrder",
            PaymentMethodTypes = ["card"],
            Metadata = new Dictionary<string, string>
                {
                    { "Destination", order.Destination },
                    { "PhoneNumber", order.PhoneNumber },
                    { "UserId", userId.ToString() },
                }.Concat(order.BooksAndQuantity.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value.ToString()))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            
            LineItems = books.Select(book => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (book.Price - book.Price * book.Discount / 100) * 100,
                    Currency = "USD",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = book.Name
                    },
                },
                Quantity = order.BooksAndQuantity[book.Id],
            }).ToList(),
            Mode = "payment"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Id;
    }
}