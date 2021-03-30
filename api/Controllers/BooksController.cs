using api.DTO;
using api.Interfaces;
using AutoMapper;
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
    }
}
