using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BseScraperAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BseScraperController : ControllerBase
    {
        private readonly ILogger<BseScraperController> _logger;

        public BseScraperController(ILogger<BseScraperController> logger)
        {
            _logger = logger;
        }

        [HttpGet("xbrl-links")]
        public async Task<IActionResult> FetchXbrlLinks()
        {
            //try
            //{
                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });

                var page = await browser.NewPageAsync();

                // Step 1: Navigate to the BSE announcements page
                await page.GotoAsync("https://www.bseindia.com/corporates/ann.html");
                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

                // Step 2: Fill form fields
                var fromDate = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
                var toDate = DateTime.Now.ToString("dd/MM/yyyy");
                await page.FillAsync("#txtFromDt", fromDate);
                await page.FillAsync("#txtToDt", toDate);

                await page.SelectOptionAsync("#ddlPeriod", "Corp. Action"); // Example: Select Category
                await page.SelectOptionAsync("#ddlsubcat", "Dividend");    // Example: Select Subcategory
          //    await page.Locator("input[type='submit'][value='Submit']").FocusAsync();
              //await page.Locator("input[type='submit'][value='Submit']").ClickAsync();

                // Step 3: Click Submit and wait for results to load
                //await page.ClickAsync("input[type='submit'][value='Submit']");
                //await page.WaitForSelectorAsync("#btnSubmit"); // Example selector
                // Example: Click the Search or Submit Button
                // Inspect the page to find the actual button selector
                //await page.ClickAsync("#btnSubmit"); // Example ID
                //await page.WaitForSelectorAsync(".table-responsive");

                // Step 4: Extract HTML content
                var content = await page.ContentAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);

                // Step 5: Parse HTML to extract XBRL links by headings
                var xbrlLinksByHeading = ExtractXbrlLinksByHeading(htmlDoc);

                return Ok(xbrlLinksByHeading);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error fetching XBRL links.");
            //    return StatusCode(500, "An error occurred while processing your request.");
            //}
        }

        private Dictionary<string, List<string>> ExtractXbrlLinksByHeading(HtmlDocument htmlDoc)
        {
            var result = new Dictionary<string, List<string>>();

            // Find all section headers (div with header and its following content)
            var sectionNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'header')]");
            if (sectionNodes != null)
            {
                foreach (var sectionNode in sectionNodes)
                {
                    // Extract heading text
                    var heading = sectionNode.InnerText.Trim();

                    // Locate the sibling container that holds XBRL links
                    var contentNode = sectionNode.SelectSingleNode("following-sibling::div");
                    if (contentNode != null)
                    {
                        var xbrlLinks = new List<string>();

                        // Find all XBRL links within this section
                        var linkNodes = contentNode.SelectNodes(".//a[contains(text(), 'XBRL')]");
                        if (linkNodes != null)
                        {
                            foreach (var linkNode in linkNodes)
                            {
                                var relativeLink = linkNode.GetAttributeValue("href", "");
                                if (!string.IsNullOrEmpty(relativeLink))
                                {
                                    var fullLink = $"https://www.bseindia.com{relativeLink}";
                                    xbrlLinks.Add(fullLink);
                                }
                            }
                        }

                        result[heading] = xbrlLinks;
                    }
                }
            }

            return result;
        }
    }
}
