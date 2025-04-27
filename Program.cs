using JobBoard.Data;
using JobBoard.Data.Interceptors;
using JobBoard.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173") // FE chạy port 5173 (bạn sửa lại cho đúng FE)
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials()); // nếu cần gửi cookie, token
});

// Các service khác
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✨ Thêm CORS đúng vị trí
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run(); // ⚡ app.Run() luôn phải là dòng CUỐI cùng
