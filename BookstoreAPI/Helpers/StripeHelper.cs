using BookstoreAPI.Models;
using BookstoreAPI.Settings;
using Microsoft.Extensions.Options;
using Stripe;
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
        string thisApiUrl = _config.GetSection("Url:Server").Value;
        string clientUrl = _config.GetSection("Url:Client").Value;
        
        string description = "You are buying: " + string.Join(", ", books.Select(book => book.Name));
        int total = books.Sum(book => book.Price * idQuantity[book.Id]);
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{thisApiUrl}/order/success?sessionId=" + "{CHECKOUT_SESSION_ID}",
            CancelUrl = clientUrl + "/failure",
            PaymentMethodTypes = new List<string>
            {
                "card"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = total,
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Bookstore Product",
                            Description = description
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment"
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Id;
    }
}