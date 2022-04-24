
using System.Text.Json.Serialization;

namespace EscrowTabWebAPI
{
    public class RequestUpdateNode
    {
        [JsonPropertyName("current-id")]
        public int NewParentId { get; set; }
    }
}
