
using Microsoft.EntityFrameworkCore;
using ProductInventory.Business.Services;
using ProductInventory.Data;
using ProductInventory.Data.Repositors;
using ProductInventory.Entities.Interfaces;

namespace ProductInventoryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            /*
            var dbfolderPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Data"));
            if (!Directory.Exists(dbfolderPath))
            {
                Directory.CreateDirectory(dbfolderPath);
            }
            var dbFilePath = Path.Combine(dbfolderPath, "ProductInventory.mdf");

            //var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=ProductInventory;AttachDbFilename={dbFilePath};Trusted_Connection=True;MultipleActiveResultSets=true";
            var connString = builder.Configuration.GetConnectionString("DBConn")!;
            connString = connString.Replace("dbFilePath", dbfolderPath);
            builder.Services.AddDbContext<ApiDBContext>(options =>
                options.UseSqlServer(connString));
            */
            // Add services to the container.
            builder.Services.AddDbContext<ApiDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn")));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Aplicar migraciones automáticamente en entorno Docker
            // crear BD automáticamente al iniciar el contenedor
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApiDBContext>();
                db.Database.EnsureCreated(); // Para producción se debe usar db.Database.Migrate();
            }

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
}
