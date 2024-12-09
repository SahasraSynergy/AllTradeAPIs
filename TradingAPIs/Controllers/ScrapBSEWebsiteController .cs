using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net.Http;
using TradingAPIs.Controllers;

namespace ScraperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BseScraperController : ControllerBase
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<BseScraperController> _logger;

        public BseScraperController(ILogger<BseScraperController> logger)
        {
            _logger = logger;
        }
        [HttpGet("scrape")]
        public async Task<IActionResult> ScrapeData()
        {
            // Prepare URL and parameters
            var url = "https://www.bseindia.com/corporates/ann.html";
            // Calculate date range
            var toDate = DateTime.Now;
            var fromDate = toDate.AddMonths(-1);

            // Prepare POST data
            var formData = new Dictionary<string, string>
                {
                    { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                    { "ToDate", toDate.ToString("dd/MM/yyyy") },
                    { "Category", "-1" }, // Default category
                    { "AnnType", "C" }    // Default announcement type
                };


            var content = new FormUrlEncodedContent(formData);

            // Send POST request to fetch filtered HTML
            //var response = await _httpClient.PostAsync(targetUrl, content);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("User-Agent", "Mozilla/5.0");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request);

           // var response = await _httpClient.PostAsync(url,null);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from BSE");
            }

            var html = await response.Content.ReadAsStringAsync();

            // Parse HTML using HtmlAgilityPack
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Extract specific data (example)
            var announcements = htmlDoc.DocumentNode
                .SelectNodes("//div[@class='announcement-text']")
                ?.Select(node => node.InnerText.Trim())
                .ToList();

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", Path.GetFullPath("Downloads")); // Set download directory
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true); // Auto-download PDFs

            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);

            try
            {
                // Find the element using XPath
                var element = driver.FindElement(By.XPath("//span[contains(@class, 'ng-binding') and text()='0.58 MB']"));

                // Click the element to download the PDF
                element.Click();

                Console.WriteLine("Download initiated. Check the specified folder.");

                if (announcements == null || announcements.Count == 0)
                {
                    return NotFound("No announcements found");
                }

               
            }
            catch (Exception ex) { }
            return Ok(announcements);
        }
 
    }
}
