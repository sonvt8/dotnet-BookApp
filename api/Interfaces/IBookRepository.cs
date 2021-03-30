using api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IBookRepository
    {
        void UpdateBook(Book book);
        void DeleteBook(Book book);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(string catename);
    }
}
