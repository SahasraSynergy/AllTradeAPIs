using System.Text;
using iText.Kernel.Pdf.Canvas.Parser;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using PdfReader = iText.Kernel.Pdf.PdfReader;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Forms;


namespace ScraperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfScraperController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
       // private static readonly string apiUrl = "https://your-api-endpoint.com"; // URL to fetch the PDF from
        private static readonly string openAiApiKey = "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"; // Your OpenAI API key
        [HttpPost("readsummary")]
        public async Task<IActionResult?>  ScrapePdf(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Instantiate OpenAIProcessor
                OpenAIProcessor openAIProcessor = new OpenAIProcessor(openAiApiKey);
                using var stream = pdfFile.OpenReadStream();
                using var memoryStream = new MemoryStream();

                stream.CopyTo(memoryStream);

                string extractedText = ExtractTextFromPdf(memoryStream);
                // Get OpenAI Response
                string smartText = await openAIProcessor.ProcessText(extractedText);
                return Ok(new { Text = smartText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            StringBuilder textBuilder = new StringBuilder();
            pdfStream.Position = 0;
            // Open the PDF document
            using (var reader = new PdfReader(pdfStream))
            {
                using (var pdfDocument = new PdfDocument(reader)) {
                    // Iterate through each page of the PDF and extract text
                    for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                    {
                        // Extract text from the page
                        var page = pdfDocument.GetPage(pageNumber);
                        string pageText = PdfTextExtractor.GetTextFromPage(page);
                        textBuilder.AppendLine(pageText);
                    }
                }
            }

            return textBuilder.ToString();
        }
    }


public class PdfTextExtractorUtil
{
    public static string ExtractText(string filePath)
    {
        StringBuilder text = new StringBuilder();

        using (PdfReader reader = new PdfReader(filePath))
        using (PdfDocument pdfDoc = new PdfDocument(reader))
        {
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                text.Append(pageText);
            }
        }

        return text.ToString();
    }
}



public class OpenAIProcessor
{
    private readonly IOpenAIService _openAIService;

    public OpenAIProcessor(string apiKey)
    {
        _openAIService = new OpenAIService(new OpenAIOptions { ApiKey = apiKey });
    }

    public async Task<string?> ProcessText(string inputText)
    {
        // Create a completion request
        var completionResult = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest
        {
            Prompt = inputText,
            Model = "text-davinci-003",
            MaxTokens = 500
        });

        // Return the first response
        return completionResult?.Choices?.FirstOrDefault()?.Text.Trim();
    }
}
}