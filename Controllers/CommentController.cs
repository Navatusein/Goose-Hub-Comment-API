using CommentAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CommentAPI.Controllers
{
    /// <summary>
    /// Comment Controller
    /// </summary>
    [Route("/api/comment-api/v1")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <remarks>Delete comment by comment id</remarks>
        /// <param name="id">Comment Id</param>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete]
        [Route("comment/{id}")]
        [Authorize(Roles = "User,Admin")]
        [SwaggerResponse(statusCode: 403, type: typeof(ErrorDto), description: "Forbidden")]
        public async Task<IActionResult> DeleteCommentId([FromRoute(Name = "id")] string id)
        {

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);
            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403, default(ErrorDto));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Comments
        /// </summary>
        /// <remarks>Get comments for movie, anime or serial</remarks>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("content/{id}")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<CommentDto>), description: "OK")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorDto), description: "Not Found")]
        public async Task<IActionResult> GetContentId([FromRoute(Name = "id")] string id)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<CommentDto>));
            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(ErrorDto));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Add Comment
        /// </summary>
        /// <remarks>Add comments for movie, anime or serial</remarks>
        /// <param name="id"></param>
        /// <param name="commentDto"></param>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpPost]
        [Route("content/{id}")]
        [Authorize(Roles = "User,Admin")]
        [SwaggerResponse(statusCode: 201, type: typeof(CommentDto), description: "Created")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorDto), description: "Not Found")]
        public async Task<IActionResult> PostContentId([FromRoute(Name = "id")] string id, [FromBody] CommentDto commentDto)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(201, default(CommentDto));
            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(ErrorDto));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Reply Comment
        /// </summary>
        /// <remarks>Reply on comment</remarks>
        /// <param name="commentId"></param>
        /// <param name="commentDto"></param>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpPost]
        [Route("reply/{commentId}")]
        [Authorize(Roles = "User,Admin")]
        [SwaggerResponse(statusCode: 201, type: typeof(CommentDto), description: "Created")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorDto), description: "Not Found")]
        public async Task<IActionResult> PostReplyCommentId([FromRoute(Name = "commentId")] string commentId, [FromBody] CommentDto commentDto)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(201, default(CommentDto));
            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(ErrorDto));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get User Comments
        /// </summary>
        /// <remarks>Get all user comments</remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("user")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<CommentDto>), description: "OK")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.Claims.First(x => x.Type == "UserId").ToString();

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<CommentDto>));

            throw new NotImplementedException();
        }
    }
}
