namespace Data_Access.DataModels.Models;

public class Bookshelf
{
    public Bookshelf()
    {
        
    }
    
    public Guid Id { get; init; }
    
    public string Name { get; set; }
    
    public int CreatedOn { get; set; } = DateTime.UtcNow.Year;
    
    public IEnumerable<Book> Books { get; set; }
}