using DataLayer;  // Your namespace for ProductService
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly AnnouncementService _announcementService;

    // AnnouncementService injected via DI
    public AnnouncementsController(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnnouncements()
    {
        try
        {
            var announcement = await _announcementService.GetAllAnnouncements();
            return Ok(new { Success = true, Data = announcement });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = "An error occurred while fetching announcements.", Details = ex.Message });
        }


    }

    [HttpPost]
    public async Task<IActionResult> CreateAnnouncement([FromBody] Announcements announcement)
    {
        await _announcementService.CreateAnnouncements(announcement);
        return CreatedAtAction(nameof(GetAnnouncements), new { id = announcement.Id }, announcement);
    }
}
