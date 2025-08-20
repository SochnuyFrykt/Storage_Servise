using Data_Access.DataModels.Models;

namespace Data_Access.Sevices;

public interface IObjectService<T>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> CreateAsync(T newShelf);
    Task<T?> UpdateAsync(Guid id, T updatedShelf);
    Task<bool> DeleteAsync(Guid id);
}