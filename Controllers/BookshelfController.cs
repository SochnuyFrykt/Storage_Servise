using Data_Access.DataModels.Models;
using Data_Access.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookshelfController: ControllerBase
{
    private readonly IObjectService<Bookshelf> _bookshelfService;
    
    public BookshelfController(IObjectService<Bookshelf> bookshelfService)
    {
        _bookshelfService = bookshelfService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShelfById(Guid id)
    {
        var shelf = await _bookshelfService.GetByIdAsync(id);
        if (shelf == null)
        {
            return NotFound(shelf);
        }
        
        return Ok(shelf);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shelfs = await _bookshelfService.GetAllAsync();
        return Ok(shelfs);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Bookshelf bookshelf)
    {
        var createdShelf = await _bookshelfService.CreateAsync(bookshelf);
        return CreatedAtAction(nameof(GetShelfById), new {id = createdShelf.Id}, createdShelf);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Bookshelf bookshelf)
    {
        var updatedBookshelf = await _bookshelfService.UpdateAsync(id, bookshelf);
        if (updatedBookshelf == null)
        {
            return NotFound(updatedBookshelf);
        }
        
        return Ok(updatedBookshelf);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        var success = await _bookshelfService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}