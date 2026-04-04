using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class RegisterVisitor
{
    [FunctionName("registerVisitor")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registerVisitor")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("New visitor registration received.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<VisitorClass>(requestBody);

        if (data == null || string.IsNullOrEmpty(data.Name))
        {
            return new BadRequestObjectResult("Name is required");
        }

        // LOGGA (Application Insights sker automatiskt via log.LogInformation)
        log.LogInformation($"Visitor registered: {data.Name}");

        return new OkObjectResult("Visitor saved");
    }
}
