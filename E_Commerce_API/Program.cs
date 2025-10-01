using Data;
using E_Commerce_API.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Models.Domain;
using Models.DTO.Wallet;
using Serilog; // <-- إضافة مهمة
using System.Text;
using Unit_Of_Work;

namespace E_Commerce_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. إعداد Serilog Configuration في البداية
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
               // هنا نقوم بتجاهل السجلات غير الهامة من .NET Core
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)

                .Enrich.FromLogContext() // لإضافة معلومات سياقية مثل RequestId
                .WriteTo.Console() // لعرض السجلات في الـ Console
                .WriteTo.File(
                    "logs/e-commerce-api-.txt",
                    rollingInterval: RollingInterval.Day, // إنشاء ملف جديد كل يوم
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}") // شكل السجل
                .CreateLogger();

            try
            {
                Log.Information("Starting web application");

                var builder = WebApplication.CreateBuilder(args);

                // 2. إخبار التطبيق باستخدام Serilog
                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddControllers();
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
                builder.Services.AddScoped<CartServices>();

                // use memory cach
                builder.Services.AddMemoryCache();

                // use redis
                builder.Services.AddStackExchangeRedisCache(op =>
                {
                    op.Configuration = "localhost:6379";
                });


                builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
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

                var jwtSection = builder.Configuration.GetSection("JWT").Get<JWToption>();
                builder.Services.AddSingleton(jwtSection);
                builder.Services.AddAuthentication(op => op.DefaultAuthenticateScheme = "my scheme")
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
                    app.UseSwaggerUI(c => c.SwaggerEndpoint(url: "/openapi/v1.json", "v1"));
                }

                // إضافة Middleware من Serilog لتسجيل تفاصيل كل طلب HTTP
                app.UseSerilogRequestLogging();

                app.UseStaticFiles();

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
            catch (Exception ex)
            {
                // تسجيل أي خطأ فادح يمنع التطبيق من البدء
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                // التأكد من حفظ جميع السجلات قبل إغلاق التطبيق
                Log.CloseAndFlush();
            }
        }
    }
}