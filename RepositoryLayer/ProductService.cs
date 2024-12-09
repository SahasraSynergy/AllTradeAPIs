using DatabaseLayer;
using DataLayer;  // Your namespace for AppDbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly AppDbContext _context;

    // Inject AppDbContext via DI
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    // Get all products from the database
    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // Create a new product and save it to the database
    public async Task CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
