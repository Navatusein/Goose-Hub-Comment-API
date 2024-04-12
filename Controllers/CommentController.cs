using AutoMapper;
using CommentAPI.Dtos;
using CommentAPI.MassTransit.Events;
using CommentAPI.MassTransit.Responses;
using CommentAPI.Models;
using CommentAPI.Service.DataService;
using MassTransit;
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
        private static Serilog.ILogger Logger => Serilog.Log.ForContext<CommentController>();

        private readonly IMapper _mapper;
        private readonly CommentService _dataService;
        private readonly IRequestClient<ContentExistEvent> _clientContentExist;
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentController(IMapper mapper, CommentService dataService, IRequestClient<ContentExistEvent> clientContentExist, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _dataService = dataService;
            _clientContentExist = clientContentExist;
            _publishEndpoint = publishEndpoint;
        }

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
        [SwaggerResponse(statusCode: 200, description: "OK")]
        [SwaggerResponse(statusCode: 403, type: typeof(ErrorDto), description: "Forbidden")]
        public async Task<IActionResult> DeleteCommentId([FromRoute(Name = "id")] string id)
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value.ToString();

            var model = await _dataService.GetAsync(id);

            if (model == null)
                return StatusCode(404, new ErrorDto("Comment not found", "404"));

            if (model.UserId != userId)
                return StatusCode(403, new ErrorDto("Forbidden", "403"));

            await _dataService.DeleteAsync(id);

            return StatusCode(200);
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
        public async Task<IActionResult> GetContentId([FromRoute(Name = "id")] string id, CancellationToken cancellationToken)
        {
            var contentExistEvent = new ContentExistEvent()
            {
                ContentId = id
            };

            var result = await _clientContentExist.GetResponse<ContentExistResponse>(contentExistEvent, cancellationToken);

            if (!result.Message.IsExists)
                return StatusCode(404, new ErrorDto("Content not found", "404"));

            var models = await _dataService.GetCommentsByContentIdAsync(id);
            var dtos = models.Select(x => _mapper.Map<CommentDto>(x)).ToList();

            return StatusCode(200, dtos);
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
        public async Task<IActionResult> PostContentId([FromRoute(Name = "id")] string id, [FromBody] CommentDto commentDto, CancellationToken cancellationToken)
        {
            var contentExistEvent = new ContentExistEvent() 
            { 
                ContentId = id 
            };

            var result = await _clientContentExist.GetResponse<ContentExistResponse>(contentExistEvent, cancellationToken);

            if (!result.Message.IsExists)
                return StatusCode(404, new ErrorDto("Content not found", "404"));

            var model = await _dataService.AddCommentAsync(_mapper.Map<Comment>(commentDto));
            
            return StatusCode(201, model);
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
            var model = await _dataService.ReplyCommentAsync(commentId, _mapper.Map<Comment>(commentDto));

            if (model == null)
                return StatusCode(404, new ErrorDto("Comment not found", "404"));

            var dto = _mapper.Map<CommentDto>(model);

            return StatusCode(200, dto);
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
            var userId = User.Claims.First(x => x.Type == "UserId").Value.ToString();

            var models = await _dataService.GetCommentsByUserIdAsync(userId);
            var dtos = models.Select(x => _mapper.Map<CommentDto>(x)).ToList();

            return StatusCode(200, dtos);
        }
    }
}
