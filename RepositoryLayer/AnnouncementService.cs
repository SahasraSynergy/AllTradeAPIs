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
        _context = context;
    }

    // Get all products from the database
    public async Task<List<Announcements>> GetAllAnnouncements()
    {
        return await _context.Announcements.ToListAsync();
    }

    // Create a new product and save it to the database
    public async Task CreateAnnouncements(Announcements announcement)
    {
        announcement.AnnounementDate =
     announcement.AnnounementDate != null
         ? announcement.AnnounementDate.Value.ToUniversalTime()
         : DateTime.UtcNow;
        announcement.DividentRecordDate =
         announcement.DividentRecordDate.ToUniversalTime();
        _context.Announcements.Add(announcement);
        await _context.SaveChangesAsync();
    }
}
