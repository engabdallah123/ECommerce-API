using Data;
using E_Commerce_API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Domain;
using Models.DTO.Wallet;
using System.Text;
using Unit_Of_Work;

namespace E_Commerce_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies();
            });
              
          
            builder.Services.AddScoped<UnitWork>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<WalletService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;

                // محتاج رقم؟
                options.Password.RequireDigit = false;

                // محتاج حروف كابيتال؟
                options.Password.RequireUppercase = false;

                // محتاج حروف سمول؟
                options.Password.RequireLowercase = false;

                // محتاج رموز خاصة؟
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();

                    });
            });
            builder.Services.AddControllers();
            var jwtSection = builder.Configuration.GetSection("JWT").Get<JWToption>(); // get the JWToption section from appsettings.json
            builder.Services.AddSingleton(jwtSection); // عشان تتبعت للكونستركتر بتاع الcontroller
            builder.Services.AddAuthentication(op=>op.DefaultAuthenticateScheme="my scheme")
                .AddJwtBearer("my scheme", options =>
                {
                    options.SaveToken = true;
                    string secretKey = jwtSection.SecretKey;
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = key,
                       

                    };
                });
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(c=> c.SwaggerEndpoint(url:"/openapi/v1.json", "v1"));
            }


            app.UseStaticFiles(); // for wwwroot

            // for make images is available with url
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")),
                RequestPath = "/uploads"
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();          
            app.MapControllers();

            app.Run();
        }
    }
}
