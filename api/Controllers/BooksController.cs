using api.DTO;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public BooksController(IBookRepository bookRepository, IMapper mapper, IPhotoService photoService)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookRepository.GetBooksAsync();

            return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        [AllowAnonymous]
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

        [Authorize(Policy = "ModerateAdminRole")]
        [HttpPost("add-photo/{bookId}")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) return BadRequest("Book does not exist!");

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null)
            {
                Console.WriteLine("check");
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (book.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            book.Photos.Add(photo);

            if (await _bookRepository.SaveAllAsync())
            {
                return _mapper.Map<PhotoDto>(photo);
            }

            return BadRequest("Problem addding photo");
        }
    }
}
