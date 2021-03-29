using api.DTO;
using api.Entities;
using AutoMapper;

namespace api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUsers, AuthorDto>();
            CreateMap<Book, BookDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Photo, PhotoDto>();
        }
    }
}
