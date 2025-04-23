using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DTO.Wallet;
using System.Text;
using Unit_Of_Work;

namespace E_Commerce_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string AllowAll = "AllowAll";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
           
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddScoped<UnitWork>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAll,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddAuthentication(op=>op.DefaultAuthenticateScheme="my scheme")
                .AddJwtBearer("my scheme", options =>
                {
                    string secretKey = "you can't see me ya aaroooo 123456789";
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = key,
                        
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                // app.UseSwaggerUI(c=> c.SwaggerEndpoint( "/openapi/v1.json", "v1"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(AllowAll);
            app.MapControllers();

            app.Run();
        }
    }
}
