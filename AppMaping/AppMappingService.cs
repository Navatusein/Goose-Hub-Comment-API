using AutoMapper;
using CommentAPI.Dtos;
using CommentAPI.Models;

namespace CommentAPI.AppMaping
{
    /// <summary>
    /// AppMappingService
    /// </summary>
    public class AppMappingService:Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppMappingService()
        {
            CreateMap<Comment, CommentDto>()
                .ReverseMap();
        }
    }
}
