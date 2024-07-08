using BookstoreAPI.DapperRequests;
using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Stripe.Checkout;

namespace BookstoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PayController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly StripeHelper _stripeHelper;

        public PayController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
            _stripeHelper = new StripeHelper(_config);
        }

        [AllowAnonymous]
        [HttpPost("makeOrder")]
        public async Task<IActionResult> CreateOrder(OrderDTO order)
        {
            int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
            string sqlGetBooks = BookRequest.GetSomeBooks;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BooksId", order.BooksAndQuantity.Keys.ToArray(), System.Data.DbType.Object);
            IEnumerable<BookGenerallyInfo> books = _dapper.LoadDataWithParameters<BookGenerallyInfo>(sqlGetBooks, parameters);

            foreach (BookGenerallyInfo book in books) if (book.AvailableQuantity < order.BooksAndQuantity[book.Id]) throw new Exception("Not enough books in stock");
            
            string id = await _stripeHelper.CheckOut(books, order, userId);
            
            return Ok(id);
        }
        
        
        [AllowAnonymous]
        [HttpGet("success/{sessionId}")]
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            var idQuantity = session.Metadata
                .Where(kvp => int.TryParse(kvp.Key, out _))
                .ToDictionary(kvp => int.Parse(kvp.Key), kvp => int.Parse(kvp.Value));

            var destination = session.Metadata["Destination"];
            var phoneNumber = session.Metadata["PhoneNumber"];
            int userId = int.Parse(session.Metadata["UserId"]);

            string sqlCreateOrder = PayRequest.CreatePayment;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BooksId", idQuantity.Keys.ToArray(), System.Data.DbType.Object);
            parameters.Add("@OrderedQuantity", idQuantity.Values.ToArray(), System.Data.DbType.Object);
            parameters.Add("@UserId", userId, System.Data.DbType.Int32);
            parameters.Add("@Destination", destination, System.Data.DbType.String);
            parameters.Add("@PhoneNumber", phoneNumber, System.Data.DbType.String);
            
            _dapper.ExecuteSqlWithParameters(sqlCreateOrder, parameters);
            return Redirect(_config.GetSection("Urls:Client").Value);
        }
    }
}