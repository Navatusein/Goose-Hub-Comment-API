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
    [Route("v1/comments")]
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

            dtos.ForEach(x =>
            {
                x.Replies = dtos.Where(y => y.Id == x.ParentId).Select(y => y.Id!).ToList();
            });

            return StatusCode(200, dtos);
        }

        /// <summary>
        /// Add Comment
        /// </summary>
        /// <remarks>Add comments for movie, anime or serial</remarks>
        /// <param name="commentDto"></param>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpPost]
        [Route("content")]
        [Authorize(Roles = "User,Admin")]
        [SwaggerResponse(statusCode: 201, type: typeof(CommentDto), description: "Created")]
        [SwaggerResponse(statusCode: 404, type: typeof(ErrorDto), description: "Not Found")]
        public async Task<IActionResult> PostContentId([FromBody] CommentDto commentDto, CancellationToken cancellationToken)
        {
            var contentExistEvent = new ContentExistEvent() 
            { 
                ContentId = commentDto.ContentId
            };

            var result = await _clientContentExist.GetResponse<ContentExistResponse>(contentExistEvent, cancellationToken);

            if (!result.Message.IsExists)
                return StatusCode(404, new ErrorDto("Content not found", "404"));

            if (commentDto.ParentId != null)
            {
                var parent = await _dataService.GetAsync(commentDto.ParentId!);

                if (parent == null)
                    return StatusCode(404, new ErrorDto("Comment not found", "404"));
            }

            var model = await _dataService.AddCommentAsync(_mapper.Map<Comment>(commentDto));
            
            return StatusCode(201, model);
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
