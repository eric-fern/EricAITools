using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Aspire.Azure.Storage;

namespace tools.ApiService
{
    // ContentController.cs
    public class ContentController : ControllerBase
    {
        private readonly BlobContainerClient _blobContainer;
        private readonly ILogger<ContentController> _logger;

        public ContentController(
            BlobServiceClient blobServiceClient,
            ILogger<ContentController> logger)
        {
            // Creates or gets a reference to our container
            _blobContainer = blobServiceClient.GetBlobContainerClient("content-store");
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadContent([FromForm] IFormCollection formData)
        {
            // Generate a unique request ID that will organize all content from this upload
            string requestId = Guid.NewGuid().ToString("N");

            try
            {
                // Dictionary to track all the content we store
                var uploadedContent = new Dictionary<string, string>();

                // Process any files in the request
                foreach (var file in formData.Files)
                {
                    // Create a path that includes the request ID for organization
                    string blobPath = $"{requestId}/{file.FileName}";
                    var blobClient = _blobContainer.GetBlobClient(blobPath);

                    // Upload the file content
                    using var stream = file.OpenReadStream();
                    await blobClient.UploadAsync(stream, overwrite: true);

                    uploadedContent.Add(file.Name, blobClient.Uri.ToString());
                    _logger.LogInformation("Uploaded file {FileName} to blob storage", file.FileName);
                }

                // Process any text fields
                foreach (var field in formData.Keys.Where(k => !formData.Files.Any(f => f.Name == k)))
                {
                    string blobPath = $"{requestId}/text_{field}.txt";
                    var blobClient = _blobContainer.GetBlobClient(blobPath);

                    var textContent = formData[field].ToString();
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));
                    await blobClient.UploadAsync(stream, overwrite: true);

                    uploadedContent.Add(field, blobClient.Uri.ToString());
                }

                // Store metadata about this upload
                var metadata = new
                {
                    RequestId = requestId,
                    Timestamp = DateTime.UtcNow,
                    ContentUrls = uploadedContent
                };

                var metadataBlob = _blobContainer.GetBlobClient($"{requestId}/metadata.json");
                await metadataBlob.UploadAsync(
                    new MemoryStream(
                        Encoding.UTF8.GetBytes(JsonSerializer.Serialize(metadata))
                    ),
                    overwrite: true
                );

                return Ok(new
                {
                    requestId,
                    urls = uploadedContent,
                    message = "Content uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading content for request {RequestId}", requestId);
                return StatusCode(500, "An error occurred while processing your upload");
            }
        }
    }
}
