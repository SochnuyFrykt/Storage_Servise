namespace Data_Access.DataModels.Models;

public class Book
{
    public Book()
    {
        
    }
    
    public Guid Id { get; init; }
    
    public string Name { get; set; }
    
    public string Author { get; set; }
    
    public string ISBN { get; set; }
    
    public string YDK { get; set; }
    
    public string BBK { get; set; }
    
    public string Description { get; set; }
    
    public int YearPublication { get; set; }
    
    public Guid? BookShelfId { get; set; }
    
    public Bookchself Bookshelf { get; set; }
}