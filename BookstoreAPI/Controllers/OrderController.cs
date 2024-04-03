using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using Stripe.Checkout;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly StripeHelper _stripeHelper;

        public OrderController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
            _stripeHelper = new StripeHelper(_config);
        }


        [HttpPost("makeOrder")]
        public async Task<IActionResult> CreateOrder(IDictionary<int, int> idQuantity)
        {
            string sqlGetBooks = @"SELECT * FROM book_schema.bookgenerallyInfo WHERE BookGenerallyInfo.id = ANY (@BooksId)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BooksId", idQuantity.Keys.ToArray(), System.Data.DbType.Object);
            IEnumerable<BookGenerallyInfo> books = _dapper.LoadDataWithParameters<BookGenerallyInfo>(sqlGetBooks, parameters);
            
            string id = await _stripeHelper.CheckOut(books, idQuantity);
            string pubKey = _config["StripeSettings:PubKey"];
            
            var dataOrder = new CheckoutOrderResponseDTO()
            {
                SessionId = id,
                PubKey = pubKey
            };
            return Ok(dataOrder);
        }
        
        
        [HttpGet("success/{sessionId}")]
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);
            
            IDictionary<int, int> idQuantity = session.Metadata.ToDictionary(kvp => int.Parse(kvp.Key), kvp => int.Parse(kvp.Value));
            
            return Redirect(_config.GetSection("Urls:Client").Value);
        }
        
    }
}