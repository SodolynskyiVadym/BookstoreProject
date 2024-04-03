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
    
    public async Task<string> CheckOut(IEnumerable<BookGenerallyInfo> books, IDictionary<int, int> idQuantity)
    {
        string? serverUrl = _config.GetSection("Urls:Server").Value;
        string? clientUrl = _config.GetSection("Urls:Client").Value;
        
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{serverUrl}/order/success/{{CHECKOUT_SESSION_ID}}",
            CancelUrl = clientUrl,
            PaymentMethodTypes = ["card"],
            Metadata = idQuantity.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value.ToString()),
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
                Quantity = idQuantity[book.Id],
            }).ToList(),
            Mode = "payment"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Id;
    }
}