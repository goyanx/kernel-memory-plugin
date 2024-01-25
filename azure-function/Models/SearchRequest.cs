using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models;

public class SearchRequest
{
    [JsonPropertyName("index")]
    public string Index { get; set; }

    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("filters")]
    public List<Dictionary<string, List<string>>> Filters { get; set; }

    [JsonPropertyName("minRelevance")]
    public int MinRelevance { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}
