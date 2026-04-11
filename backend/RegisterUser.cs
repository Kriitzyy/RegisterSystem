using System;   
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
public static class RegisterVisitor
{
    [FunctionName("registerVisitor")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "registerVisitor")] HttpRequest req,
        ILogger log)
    {
        if (req.Method == "OPTIONS")
        {
            var response = new OkResult();
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            req.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return response;
        }                                                                     

        log.LogInformation("New visitor registration received.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<VisitorClass>(requestBody);

        if (data == null || string.IsNullOrEmpty(data.Name))
        {       
            return new BadRequestObjectResult("Name is required");
        }   

        try {
        string connString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=visitors";
        using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand("INSERT INTO visitors (name) VALUES (@name)", conn);
        cmd.Parameters.AddWithValue("name", data.Name);
        await cmd.ExecuteNonQueryAsync();

        } catch (Exception ex) {
            log.LogError($"Database error: {ex.Message}");
            return new StatusCodeResult(500);
        }
        log.LogInformation($"Visitor registered: {data.Name}");

        var okResult = new OkObjectResult("Visitor saved");
        req.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

        return okResult;
    }
}


/*
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
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "registerVisitor")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("New visitor registration received.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<VisitorClass>(requestBody);

        if (data == null || string.IsNullOrEmpty(data.Name))
        {
            return new BadRequestObjectResult("Name is required");
        }

        log.LogInformation($"Visitor registered: {data.Name}");

        return new OkObjectResult("Visitor saved");
    }
}
//http://localhost:7071/api/registerVisitor

*/