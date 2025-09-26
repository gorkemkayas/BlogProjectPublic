using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class SummarizeDto
    {
        [JsonPropertyName("postId")]
        public string PostId { get; set; }
    }
}
