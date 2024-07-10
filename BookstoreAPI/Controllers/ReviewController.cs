using BookstoreAPI.DapperRequests;
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
    public IEnumerable<ReviewDto> GetBookReviews(int id)
    {
        return _dapper.LoadData<ReviewDto>(ReviewRequest.GetBookReviews(id));
    }

    [HttpGet("getUserReview/{bookId}")]
    public Review? GetUserReview(int bookId)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        string sqlGetReview = ReviewRequest.GetUserReview;
        
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@BookId", bookId, System.Data.DbType.Int64);
        parameters.Add("@UserId", userId, System.Data.DbType.Int64);
        return _dapper.LoadDataSingleWithParameters<Review>(sqlGetReview, parameters);
    }
    
    
    [HttpPost("createReview")]
    public IActionResult CreateReview(ReviewCreateDto reviewCreate)
    {
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        string sqlAddReview = ReviewRequest.CreateReview;
        var parameters = new DynamicParameters();
        parameters.Add("@BookId", reviewCreate.BookId, System.Data.DbType.Int64);
        parameters.Add("@UserId", userId, System.Data.DbType.Int64);
        parameters.Add("@Description", reviewCreate.Description, System.Data.DbType.String);
        parameters.Add("@Mark", reviewCreate.Mark, System.Data.DbType.Int32);
        _dapper.ExecuteSqlWithParameters(sqlAddReview, parameters);
        return Ok();
    }
    
    
    [HttpPatch("updateReview")]
    public IActionResult UpdateReview(ReviewUpdateDto review)
    {
        int userIdOwner = _dapper.LoadDataSingle<int>($"SELECT userId FROM book_schema.Reviews WHERE id = {review.Id}");
        int userId = int.TryParse(User.FindFirst("userId")?.Value, out userId) ? userId : 0;
        if (userIdOwner != userId)
        {
            return StatusCode(400, "You are not the owner of this review");
        }

        string sqlUpdateReview = ReviewRequest.UpdateReview;
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
        string sqlDeleteReview = ReviewRequest.DeleteReview(id);
        _dapper.ExecuteSql(sqlDeleteReview);
        return Ok();
    }
}