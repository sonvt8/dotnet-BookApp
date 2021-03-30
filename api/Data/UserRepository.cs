using api.DTO;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUsers> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUsers> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUsers>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUsers users)
        {
            _context.Entry(users).State = EntityState.Modified;
        }

        //public async Task<AuthorDto> GetAuthorAsync(string username)
        //{
        //    return await _context.Users.Where(x => x.UserName == username)
        //        .Select(user => new AuthorDto
        //        {
        //            Id = user.Id,
        //            UserName = user.UserName,
        //            Age = user.GetAge(),
        //            KnownAs = user.KnownAs,
        //            Created = user.Created,
        //            LastActive = user.LastActive,
        //            Gender = user.Gender,
        //            Introduction = user.Introduction,
        //            Interests = user.Interests,
        //            City = user.City,
        //            Country = user.Country,
        //            Books = user.Books.Select(book => new BookDto
        //            {
        //                Id = book.Id,
        //                Title = book.Title,
        //                Price = book.Price,
        //                Quantity = book.Quantity,
        //                Description = book.Description,
        //                Photos = book.Photos.Select(photo => new PhotoDto
        //                {
        //                    Id = photo.Id,
        //                    Url = photo.Url,
        //                    IsMain = photo.IsMain
        //                }).ToList(),
        //                Categories = _context.Categories.Include(b => b.Books).Select(cate => new CategoryDto
        //                {
        //                    Id = cate.Id,
        //                    Name = cate.Name
        //                }).ToList()
        //            }).ToList()
        //        }).SingleOrDefaultAsync();
        //}
    }
}


