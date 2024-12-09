//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using System.Text;
//using UglyToad.PdfPig;

//namespace ScraperApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PdfScraperController : ControllerBase
//    {
//        [HttpPost("readsummary")]
//        public IActionResult ScrapePdf(IFormFile pdfFile)
//        {
//            if (pdfFile == null || pdfFile.Length == 0)
//            {
//                return BadRequest("Invalid file. Please upload a PDF file.");
//            }

//            try
//            {
//                StringBuilder extractedText = new StringBuilder();

//                // Read the PDF
//                using (var pdfStream = pdfFile.OpenReadStream())
//                using (var pdfDocument = PdfDocument.Open(pdfStream))
//                {
//                    foreach (var page in pdfDocument.GetPages())
//                    {
//                        extractedText.AppendLine(page.Text);
//                    }
//                }

//                // Process the extracted text (Example: Extract email addresses)
//                var text = extractedText.ToString();
//                var meaningfulData = ExtractMeaningfulData(text);

//                return Ok(new
//                {
//                    RawText = text,
//                    ExtractedData = meaningfulData
//                });
//            }
//            catch
//            {
//                return StatusCode(500, "Error processing the PDF file.");
//            }
//        }

//        private object ExtractMeaningfulData(string text)
//        {
//            // Example: Extract email addresses using regex
//            var emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
//            var emails = System.Text.RegularExpressions.Regex.Matches(text, emailPattern);

//            return new
//            {
//                EmailAddresses = emails.Select(e => e.Value).Distinct().ToList(),
//                WordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length
//            };
//        }
//    }
//}
