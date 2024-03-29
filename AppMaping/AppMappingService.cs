using AutoMapper;
using CommentAPI.Dto;
using CommentAPI.Models;

namespace CommentAPI.AppMaping
{
    public class AppMappingService:Profile
    {
        public AppMappingService()
        {
            CreateMap<Comment, CommentDto>()
                .ReverseMap();
        }
    }
}
