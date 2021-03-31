using api.DTO;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Authorize]
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookRepository.GetBooksAsync();

            return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        [HttpGet("{category}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBookByCategory(string category)
        {
            var books = await _bookRepository.GetBooksByCategoryAsync(category);

            return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        [Authorize(Policy = "ModerateAdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, BookCreateUpdateDto updateBookDto)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return BadRequest("Book does not exist!");

            _mapper.Map(updateBookDto, book);

            _bookRepository.UpdateBook(book);

            if (await _bookRepository.SaveAllAsync())
                return Ok("Book has been updated successfully");

            return BadRequest("Failed to update book");
        }

        [Authorize(Policy = "ModerateAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return BadRequest("Book does not exist!");

            _bookRepository.DeleteBook(book);

            if (await _bookRepository.SaveAllAsync()) return Ok("Book has been removed successfully");

            return BadRequest("Failed to delete book");
        }

        [Authorize(Policy = "ModerateAdminRole")]
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(BookCreateUpdateDto createBookDto)
        {
            var book = new Book();

            _mapper.Map(createBookDto, book);

            _bookRepository.AddBook(book);

            if (await _bookRepository.SaveAllAsync()) return Ok("Book has been created successfully");

            return BadRequest("Failed to create a new Book");
        }
    }
}
