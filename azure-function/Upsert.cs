using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Models;

namespace SimpleTodo.Plugin;

public class Upsert
{
    private static readonly HttpClient httpClient = new HttpClient();

    [Function("Upsert")]
    [OpenApiOperation(operationId: "Upsert", tags: new[] { "ExecuteFunction" }, Description = "Executes an /upsert in the Kernel-Memory service to save snippets of facts, conversation to be used anytime")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpsertRequest), Required = true, Description = "The upsert request")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UpsertResponse), Description = "The upsert request results")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "Displays an error message")]

    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        // Load app settings
        var appSettings = AppSettings.LoadSettings();

        // Read the request body and deserialize it into SearchRequest
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        UpsertRequest upsertRequest = JsonSerializer.Deserialize<UpsertRequest>(requestBody)!;

        // Define the URL to make the HTTP POST request to
        string url = "http://127.0.0.1:9001/upsert";

        // Prepare the JSON payload
        StringContent payload = new(JsonSerializer.Serialize(upsertRequest), Encoding.UTF8, "application/json");

        // Make the HTTP POST request
        HttpResponseMessage response = await httpClient.PostAsync(url, payload);

        // Prepare the response object
        var httpResponse = req.CreateResponse(HttpStatusCode.OK);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read and return the response body as string
            string responseBody = await response.Content.ReadAsStringAsync();

            UpsertResponse upsertResponse = JsonSerializer.Deserialize<UpsertResponse>(responseBody)!;

            // Serialize the filtered array to JSON
            string jsonPayload = JsonSerializer.Serialize(upsertResponse);

            // Serialize the SearchResponse back to JSON and set the response content
            httpResponse.WriteString(JsonSerializer.Serialize(jsonPayload));
            httpResponse.Headers.Add("Content-Type", "application/json");
            httpResponse.StatusCode = HttpStatusCode.OK;
        }
        else
        {
            httpResponse.StatusCode = HttpStatusCode.BadRequest;
            httpResponse.WriteString("Failed to make HTTP request.");
        }

        return httpResponse;
    }
}
