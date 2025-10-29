
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// EF Core DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("BANK_DB_CONNECTION");

builder.Services.AddDbContext<AppContext>(options =>
    options.UseSqlServer(connectionString));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add other services
builder.Services.AddScoped<UserService>();

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
