using DatabaseLayer;
using DataLayer;  // Your namespace for AppDbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnnouncementService
{
    private readonly AppDbContext _context;

    // Inject AppDbContext via DI
    public AnnouncementService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Get all products from the database
    public async Task<List<Announcements>> GetAllAnnouncements()
    {
        if(_context.Announcements !=null || _context.Announcements.Any())
        return await _context.Announcements.ToListAsync();
        return new List<Announcements>();
    }

    // Create a new product and save it to the database
    public async Task CreateAnnouncementAsync(Announcements announcement)
    {
        //   announcement.AnnounementDate =
        //announcement.AnnounementDate != null
        //    ? announcement.AnnounementDate.Value.ToUniversalTime()
        //    : DateTime.UtcNow;
        //   announcement.DividentRecordDate =
        //    announcement.DividentRecordDate.ToUniversalTime();
        //   if (_context.Announcements != null || !_context.Announcements.Any())
        //   {
        //       _context.Announcements.Add(announcement);
        //       await _context.SaveChangesAsync();
        //   }
        // Ensure the database is created (this will create the tables if they don't exist)
        await _context.Database.EnsureCreatedAsync();
        await _context.Database.MigrateAsync();
        if (announcement == null)
            throw new ArgumentNullException(nameof(announcement), "Announcement cannot be null.");

        // Ensure AnnouncementDate is set to UTC if it's not already
        announcement.AnnounementDate = announcement.AnnounementDate?.ToUniversalTime() ?? DateTime.UtcNow;

        // Ensure DividendRecordDate is set to UTC
        announcement.DividentRecordDate = announcement.DividentRecordDate.ToUniversalTime();

        if (_context.Announcements != null || !_context.Announcements.Any())
        {
            await _context.Announcements.AddAsync(announcement);  // Asynchronous add to ensure non-blocking operations
            await _context.SaveChangesAsync();  // Save changes asynchronously
        }
    }
}
