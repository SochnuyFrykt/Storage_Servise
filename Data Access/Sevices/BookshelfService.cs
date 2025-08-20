using System.Text.Json;
using Data_Access.DataModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Data_Access.Sevices;

public class BookshelfService: IObjectService<Bookshelf>
{
    private readonly LibraryContext _dbContext;
    private readonly IDistributedCache _cache;
    
    public BookshelfService(LibraryContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
    
    public async Task<Bookshelf?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"shelf:{id}";

        var cacheShelf = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheShelf))
        {
            return JsonSerializer.Deserialize<Bookshelf>(cacheShelf);
        }
        
        var shelfFromDb = await _dbContext.Bookshelfs.AsNoTracking()
            .FirstOrDefaultAsync(sh => sh.Id == id);

        if (shelfFromDb == null) return shelfFromDb;

        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        await _cache.SetStringAsync(
            cacheKey, JsonSerializer.Serialize(shelfFromDb));
        
        return shelfFromDb;
    }

    public async Task<IEnumerable<Bookshelf>> GetAllAsync()
    {
        return await _dbContext.Bookshelfs.AsNoTracking().ToListAsync();
    }

    public async Task<Bookshelf> CreateAsync(Bookshelf newShelf)
    {
        await _dbContext.Bookshelfs.AddAsync(newShelf);
        await _dbContext.SaveChangesAsync();
        return newShelf;
    }

    public async Task<Bookshelf?> UpdateAsync(Guid id, Bookshelf updatedShelf)
    {
        var shelfToUpdate = await _dbContext.Bookshelfs.FindAsync(id);
        if (shelfToUpdate == null)
        {
            return null;
        }
        
        shelfToUpdate.Name = updatedShelf.Name;
        shelfToUpdate.CreatedOn = updatedShelf.CreatedOn;

        await _dbContext.SaveChangesAsync();

        var keyCache = $"shelf:{id}";
        await _cache.RemoveAsync(keyCache);        
        
        return shelfToUpdate;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var shelfToDelete = await _dbContext.Bookshelfs.FindAsync(id);
        if (shelfToDelete == null)
        {
            return false;
        }
        
        _dbContext.Bookshelfs.Remove(shelfToDelete);
        await _cache.RemoveAsync($"shelf:{id}");

        return true;
    }
}