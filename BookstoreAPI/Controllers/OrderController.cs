using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;

        public OrderController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _dapper = new DataContextDapper(_config);
        }


        [HttpPost("makeOrder")]
        public IActionResult CreateOrder(OrderDTO order)
        {
            string sqlCreateOrder = @"CALL book_schema.spOrder_Upsert(@BooksId, @Quantities, @Destination, @UserId);";

            int tryGetUserId;
            int? userId = Int32.TryParse(this.User.FindFirst("userId")?.Value, out tryGetUserId) ? tryGetUserId : null; 

            DynamicParameters parameters = new DynamicParameters();


            parameters.Add("@BooksId", order.BooksAndQuantity.Keys.ToArray(), DbType.Object);
            parameters.Add("@Quantities", order.BooksAndQuantity.Values.ToArray(), DbType.Object);
            parameters.Add("@Destination", order.Destination, DbType.String);
            parameters.Add("@UserId", userId, DbType.Int32);

            _dapper.ExecuteSqlWithParameters(sqlCreateOrder, parameters);
            return Ok();
        }


        [HttpDelete("deleteOrder/{id}")]
        public IActionResult DeleteOrders(int id)
        {
            string sqlDeleteOrder = @"CALL book_schema.spOrder_Delete(@OrderId);";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrderId", id, DbType.Int32);

            _dapper.ExecuteSqlWithParameters(sqlDeleteOrder, parameters);
            return Ok();
        }

    }
}
