using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class DetectLanguage
    {
        private readonly ILogger<DetectLanguage> _logger;

        public DetectLanguage(ILogger<DetectLanguage> logger)
        {
            _logger = logger;
        }

        [Function("Function")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("DetectLanguage Azure Function processed request ID:");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
