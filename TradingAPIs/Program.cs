using DatabaseLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Read and validate connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("Connection String: " + connectionString);

// Configure DbContext with validated connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        x => x.MigrationsAssembly("DatabaseLayer")));
//builder.Services.AddDbContext<AppDbContext>(options =>
//    //options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//options.UseNpgsql("DefaultConnection",
//   x => x.MigrationsAssembly("DatabaseLayer")));
// Add services to the container.
// Add this in ConfigureServices method (Startup.cs) or builder.Services.Add... in Program.cs
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AnnouncementService>();

// Read allowed origins from appsettings.json
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
if (allowedOrigins != null) {
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", policy =>
        {
            //policy.WithOrigins("http://localhost:3000") // React app URL
            policy.WithOrigins(allowedOrigins) // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Automatically apply migrations
    using (var scope = app.Services.CreateScope())
    {
        //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //dbContext.Database.Migrate(); // Applies any pending migrations
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Check for pending migrations
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");

            // Generate and apply a new migration programmatically
            var migrator = scope.ServiceProvider.GetRequiredService<IMigrator>();
            foreach (var migration in pendingMigrations)
            {
                migrator.Migrate(migration);
            }

            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }
}
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
