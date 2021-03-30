using api.DTO;
using api.Entities;
using AutoMapper;

namespace api.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUsers, MemberDto>();
            CreateMap<MemberDto, AppUsers>();

            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();
        }
    }
}
