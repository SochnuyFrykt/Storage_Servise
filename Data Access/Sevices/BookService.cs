using System.Text.Json;
using Data_Access.DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Data_Access.Sevices;

public class BookService: IObjectService<Book>
{
    private readonly LibraryContext _dbContext;
    private readonly IDistributedCache _cache;

    public BookService(LibraryContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
    
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"book:{id}";

        var cachedBookJson = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cachedBookJson))
        {
            return JsonSerializer.Deserialize<Book>(cachedBookJson);
        }

        var bookFromDb = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

        if (bookFromDb == null) return bookFromDb;
        
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(bookFromDb), options);

        return bookFromDb;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbContext.Books.AsNoTracking().ToListAsync();
    }

    public async Task<Book> CreateAsync(Book newShekf)
    {
        _dbContext.Books.Add(newShekf);
        await _dbContext.SaveChangesAsync();
        return newShekf;
    }

    public async Task<Book?> UpdateAsync(Guid id, Book updatedShelf)
    {
        var bookToUpdate = await _dbContext.Books.FindAsync(id);
        if (bookToUpdate == null)
        {
            return null;
        }

        bookToUpdate.Name = updatedShelf.Name;
        bookToUpdate.Author = updatedShelf.Author;
        
        await _dbContext.SaveChangesAsync();

        var cacheKey = $"book:{id}";
        await _cache.RemoveAsync(cacheKey);

        return bookToUpdate;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bookToDelete = await _dbContext.Books.FindAsync(id);
        if (bookToDelete == null)
        {
            return false;
        }

        _dbContext.Books.Remove(bookToDelete);
        await _dbContext.SaveChangesAsync();

        var cacheKey = $"book:{id}";
        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}