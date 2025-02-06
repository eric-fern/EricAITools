using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
//Test azure http trigger function
//DO SOMETHING IF RECEIVE HTTP
namespace AzureFunctions
{
    public class HelloWorldTrigger
    {
        private readonly ILogger<HelloWorldTrigger> _logger;
        //constructor for the logger
        public HelloWorldTrigger(ILogger<HelloWorldTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HelloWorld")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            //log and print if receive http
            _logger.LogInformation("C# HTTP trigger function - HelloWorld received http");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
