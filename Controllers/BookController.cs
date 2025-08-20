using Data_Access.DataModels.Models;
using Data_Access.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IObjectService<Book> _bookService; 
    
    public BookController(IObjectService<Book> bookService)
    {
        _bookService = bookService;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book == null) 
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBook([FromBody] Book newBook)
    {
        var createdBook = await _bookService.CreateAsync(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book bookToUpdate)
    {
        var updatedBook = await _bookService.UpdateAsync(id, bookToUpdate);
        if (updatedBook == null)
        {
            return NotFound();
        }
        return Ok(updatedBook);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var success = await _bookService.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}