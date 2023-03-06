using AutoMapper;
using BLL;
using DAO;
using System.Security.Cryptography;
using System.Text;
using Web.Models;

namespace Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, TodoDto>();                
            CreateMap<Comment, CommentDto>();

            CreateMap<TodoDto, Todo>();
            CreateMap<CommentDto, Comment>();            
            CreateMap<LogDto, Log>();


            CreateMap<TodoDto, TodoWM>().
                ForMember(dest => dest.Hash,
                           o => o.MapFrom(val => GetHash(val.Title)));
            CreateMap<CommentDto, CommentWM>();
            CreateMap<TodoWM, TodoDto>();
            CreateMap<CommentWM, CommentDto>();
            CreateMap<TodoQueryWM, TodoQueryDto>();

            
        }

        string GetHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var byteHash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hash = BitConverter.ToString(byteHash).Replace("-","");
                return hash;
            }
        }
    }
}
