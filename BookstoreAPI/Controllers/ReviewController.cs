using BookstoreAPI.DTO;
using BookstoreAPI.Helpers;
using BookstoreAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    
    public ReviewController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    
    
    [AllowAnonymous]
    [HttpGet("getReviews/{id}")]
    public IEnumerable<Review> GetBookReviews(int id)
    {
        string sqlGetReviews = $@"SELECT * FROM book_schema.Reviews WHERE bookId = @Id";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);
        return _dapper.LoadDataWithParameters<Review>(sqlGetReviews, parameters);
    }
    
    [HttpPost("createReview")]
    public IActionResult CreateReview(ReviewCreateDTO reviewCreate)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        string sqlAddReview = $@"INSERT INTO book_schema.Reviews (bookId, userId, description, mark) VALUES (@BookId, @UserId, @Description, @Mark)";
        var parameters = new DynamicParameters();
        parameters.Add("@BookId", reviewCreate.BookId, System.Data.DbType.Int64);
        parameters.Add("@UserId", userId, System.Data.DbType.Int64);
        parameters.Add("@Description", reviewCreate.Description, System.Data.DbType.String);
        parameters.Add("@Mark", reviewCreate.Mark, System.Data.DbType.Int32);
        _dapper.ExecuteSqlWithParameters(sqlAddReview, parameters);
        return Ok();
    }
    
    
    [HttpPatch("updateReview")]
    public IActionResult UpdateReview(ReviewUpdateDTO review)
    {
        int userIdOwner = _dapper.LoadDataSingle<int>($"SELECT userId FROM book_schema.Reviews WHERE id = {review.Id}");
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        if (userIdOwner != userId)
        {
            return StatusCode(400, "You are not the owner of this review");
        }

        string sqlUpdateReview = @"UPDATE book_schema.Reviews SET description = @Description, mark = @Mark WHERE id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("@Id", review.Id, System.Data.DbType.Int64);
        parameters.Add("@Description", review.Description, System.Data.DbType.String);
        parameters.Add("@Mark", review.Mark, System.Data.DbType.Int32);
        _dapper.ExecuteSqlWithParameters(sqlUpdateReview, parameters);
        return Ok();
    }
    
    
    [HttpDelete("deleteReview/{id}")]
    public IActionResult DeleteReview(int id)
    {
        string sqlDeleteReview = $@"DELETE FROM book_schema.Reviews WHERE id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);
        _dapper.ExecuteSqlWithParameters(sqlDeleteReview, parameters);
        return Ok();
    }
}