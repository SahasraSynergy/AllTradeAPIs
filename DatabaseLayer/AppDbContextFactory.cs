using Microsoft.Extensions.Configuration;  // For ConfigurationBuilder
using Microsoft.Extensions.Configuration.Json;  // To load JSON files
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;


namespace DatabaseLayer
{
    // public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    // {
    // public AppDbContext CreateDbContext(string[] args)
    //{
    //string currentDir = Directory.GetCurrentDirectory();
    //string appsettingsPath = 
    //appsettingsPath = Path.Combine(currentDir, "appsettings.json");

    //// Loop through the directories above the current directory to find appsettings.json
    //while (currentDir != null)
    //{
    //    // Check if the file exists
    //    if (File.Exists(appsettingsPath))
    //    {
    //        break;
    //    }

    //    // Move up to the parent directory
    //    currentDir = Directory.GetParent(currentDir)?.FullName;
    //}

    //if (string.IsNullOrEmpty(appsettingsPath) || !File.Exists(appsettingsPath))
    //{
    //    throw new FileNotFoundException("appsettings.json not found in any of the parent directories.");
    //}
    //// Set the base path to the main project's directory
    //var configuration = new ConfigurationBuilder()
    //    .SetBasePath(currentDir) // Ensure this points to the main project directory
    //    .AddJsonFile("./appsettings.json") // Go up one directory level to the main project where appsettings.json is
    //    .Build();

    //var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

    //return new AppDbContext(optionsBuilder.Options);
    // }
    // }


    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("DefaultConnection"); // Use a hardcoded or fallback connection string

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
