
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductInventory.Business.Services;
using ProductInventory.Data;
using ProductInventory.Data.Repositors;
using ProductInventory.Entities.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductInventoryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApiDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, 
            maxRetryDelay: TimeSpan.FromSeconds(30), 
            errorNumbersToAdd: null 
            )));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductDapperRepository, ProductDapperRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    
                    options.Authority = builder.Configuration["OAuth2:Authority"]; 
                    options.Audience = builder.Configuration["OAuth2:Audience"]; 
                    options.RequireHttpsMetadata = false; // Solo para desarrollo, en producción debe ser true
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false, 
                        ValidateIssuer = true, 
                        ValidateLifetime = false, 
                        ValidIssuers = new[] {
                            "http://keycloak:8080/realms/inventory",
                            "http://localhost:8081/realms/inventory"
                        },
                    };
                                    });

            builder.Services.AddAuthorization(options =>
            {
                /*
                options.AddPolicy("ReadInventory", policy =>
                {
                    //policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "inventory.read");
                });
                options.AddPolicy("WriteInventory", policy =>
                {
                    //policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "inventory.write");
                });
                */
                options.AddPolicy("ReadInventory", policy =>
                   policy.RequireAssertion(context =>
                   {
                       var scopeClaim = context.User.FindFirst("scope")?.Value;
                       return scopeClaim != null && scopeClaim.Split(' ').Contains("inventory.read");
                   }));

                options.AddPolicy("WriteInventory", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var scopeClaim = context.User.FindFirst("scope")?.Value;
                        return scopeClaim != null && scopeClaim.Split(' ').Contains("inventory.write");
                }));
            });
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory API", Version = "v1" });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("http://localhost:8081/realms/inventory/protocol/openid-connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "inventory.read", "Permite leer el inventario" },
                                { "inventory.write", "Permite crear y actualizar productos" }
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2" //Id = "Bearer"
                            }
                        },
                        new[] { "inventory.read", "inventory.write" } //Array.Empty<string>()
                    }
                });
            });
            
            var app = builder.Build();

            // Aplicar migraciones automáticamente en entorno Docker
            // crear BD automáticamente al iniciar el contenedor
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApiDBContext>();
                db.Database.EnsureCreated(); // Para producción se debe usar db.Database.Migrate();
            }

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
