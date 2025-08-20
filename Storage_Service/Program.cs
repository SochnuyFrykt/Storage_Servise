using Data_Access;
using Data_Access.DataModels.Models;
using Data_Access.Sevices;
using Microsoft.EntityFrameworkCore;

namespace Storage_Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        var pgConnectionString = builder.Configuration.GetConnectionString("PostgreSql");
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<LibraryContext>(options =>
            options.UseNpgsql(pgConnectionString));
        
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "BookApi_";
        });
        builder.Services.AddScoped<IObjectService<Book>, BookService>();
        builder.Services.AddScoped<IObjectService<Bookshelf>, BookshelfService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}