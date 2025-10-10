using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostQuery());

                return NormalResponse(posts, "Retrieving all posts. ");

            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error occurred while processing request to retrieve all posts.";
                _logger.LogError(ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetPostsByIdAsync(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });

                if(posts == null || !posts.Any())
                    return NoContent();

                return Ok(new PostLookupResponse 
                { 
                    Posts = posts,
                    Message = $"Successfully returned post with id: {postId}"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error occurred while processing request to retrieve posts by Id.";
                _logger.LogError(ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostsByAuthorAsync(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });

                return NormalResponse(posts, $"Posts by author: {author} ");
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error occurred while processing request to retrieve posts by author.";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentQuery());

                return NormalResponse(posts, "Retrieving posts with comments.");
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error occurred while processing request to retrieve posts with comments";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("withLikes/{noOfLikes}")]
        public async Task<ActionResult> GetPostsWithLikesAsync(int noOfLikes)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostWithLikesQuery { NumberOfLikes = noOfLikes});

                return NormalResponse(posts, "Retrieving posts with likes.");
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error occurred while processing request to retrieve posts with likes";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<PostEntity> posts, string message)
        {
            if (posts == null || !posts.Any())
                return NoContent();

            var count = posts.Count();

            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"{message} Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }

    }
}
