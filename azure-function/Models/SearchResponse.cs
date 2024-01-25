using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    public class SearchResponse
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("noResult")]
        public bool NoResult { get; set; }

        [JsonPropertyName("results")]
        public List<ResultItem> Results { get; set; }
    }

    public class ResultItem
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("index")]
        public string Index { get; set; }

        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; }

        [JsonPropertyName("fileId")]
        public string FileId { get; set; }

        [JsonPropertyName("sourceContentType")]
        public string SourceContentType { get; set; }

        [JsonPropertyName("sourceName")]
        public string SourceName { get; set; }

        [JsonPropertyName("sourceUrl")]
        public string SourceUrl { get; set; }

        [JsonPropertyName("partitions")]
        public List<Partition> Partitions { get; set; }
    }

    public class Partition
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("relevance")]
        public float Relevance { get; set; }

        [JsonPropertyName("lastUpdate")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonPropertyName("tags")]
        public Dictionary<string, List<string>> Tags { get; set; }
    }
}
