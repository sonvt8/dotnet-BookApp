using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context)
        {
            _context = context;
        }
        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.Include(p => p.Photos).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.Include(p => p.Photos).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(string catename)
        {
            return await _context.Books
                .Include(p => p.Photos)
                .Include(b => b.Category)
                .Where(b => b.Category.Name == catename)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateBook(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
        }
    }
}
