using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;
using System.Xml.Linq;
using TradingAPIs;

namespace BseScraperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BseWebScraperController : ControllerBase
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<BseWebScraperController> _logger;

        public BseWebScraperController(ILogger<BseWebScraperController> logger)
        {
            _logger = logger;
        }

        [HttpGet("announcements")]
        public async Task<IActionResult> GetAnnouncements()
        {
            // Initialize Playwright
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true // Set to false if you want to see the browser
            });

            var page = await browser.NewPageAsync();

            // Navigate to the BSE India Corporate Announcements page
            await page.GotoAsync("https://www.bseindia.com/corporates/ann.html");
            // Fill the "FromDate" field
            var fromDate = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy").Replace("-", "/");
            await page.FillAsync("#txtFromDt", fromDate); // Replace with the actual selector of the "FromDate" field

            // Fill the "ToDate" field
            var toDate = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
            await page.FillAsync("#txtToDt", toDate); // Replace with the actual selector of the "ToDate" field

            // Select the "Category" field (dropdown)
            await page.SelectOptionAsync("#ddlPeriod", "Corp. Action"); // Replace with actual selector and value for "All Categories"

            // Select the "AnnType" field (dropdown)
            await page.SelectOptionAsync("#ddlsubcat", "Dividend"); // Replace with actual selector and value for Announcement Type "C"

           // await page.WaitForSelectorAsync("#btnSubmit"); // Example selector
            // Example: Click the Search or Submit Button
            // Inspect the page to find the actual button selector
           // await page.ClickAsync("#btnSubmit"); // Example ID


            // Wait for the results to load
            // await page.WaitForSelectorAsync(".table-responsive"); // Replace with the selector for the table or results container

            // Extract the results from the loaded page
            var content = await page.ContentAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);





            // Extract announcement data
            var announcements = new List<Announcement>();
            // Increase default timeout for robust operations
            // Get all the links with the text 'XBRL'
            // Get all the 'XBRL' links with the ng-click attribute
            var xbrlLinks = await page.Locator("a:has-text('XBRL')").AllAsync();

            foreach (var xbrlLink in xbrlLinks)
            {
                // Extract the `ng-click` attribute value, which contains the function with arguments
                var ngClickValue = await xbrlLink.GetAttributeAsync("ng-click");

                // If `ng-click` exists, extract the values of NEWSID and SCRIP_CD from it
                if (!string.IsNullOrEmpty(ngClickValue) && ngClickValue.Contains("fn_dwnldxbrl"))
                {
                    // Extract the arguments passed to the function (NEWSID and SCRIP_CD)
                    var regex = new System.Text.RegularExpressions.Regex(@"fn_dwnldxbrl\(([^,]+),([^,]+)\)");
                    var match = regex.Match(ngClickValue);

                    if (match.Success)
                    {
                        var newsId = await xbrlLink.GetAttributeAsync("data-newsid"); // Assuming this is available
                        var scripCode = await xbrlLink.GetAttributeAsync("data-scripcode"); // Assuming this is available


                        // Construct the URL manually
                        var fullLink = $"https://www.bseindia.com/Msource/90D/CorpXbrlGen.aspx?Bsenewid={newsId}&Scripcode={scripCode}";

                        Console.WriteLine(fullLink);
                    }
                }
            }



                // Return the announcements as JSON
                return Ok(announcements);
        }
        private async Task<XbrlData> ScrapeXbrlData(IPage page)
        {
            try
            {
                // Get the raw XML content of the XBRL page
                var content = await page.ContentAsync();

                // Parse the XML data
                var xdoc = XDocument.Parse(content);

                // Extract specific XBRL data (adjust based on requirements)
                var entityIdentifier = xdoc.Descendants("{http://www.xbrl.org/2003/instance}identifier").FirstOrDefault()?.Value;

                return new XbrlData
                {
                    EntityIdentifier = entityIdentifier,
                    RawXml = xdoc.ToString()
                };
            }
            catch (Exception)
            {
                // Handle errors during XBRL parsing
                return null;
            }
        }
    }

    

}
