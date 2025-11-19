using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? Environment.GetEnvironmentVariable("BANK_DB_CONNECTION");
}

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Database connection string is not configured. Set ConnectionStrings:DefaultConnection in appsettings.json or the env var BANK_DB_CONNECTION.");

builder.Services.AddDbContext<AppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });


var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
             ?? builder.Configuration["Jwt:Key"];
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
               ?? builder.Configuration["Jwt:Issuer"];
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                 ?? builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
    throw new InvalidOperationException("JWT configuration is missing.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("Program");
    try
    {
        logger.LogInformation("Applying database migrations...");
        var db = services.GetRequiredService<AppContext>();
        db.Database.Migrate();
        logger.LogInformation("Database migration finished.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration failed.");
        throw;
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
