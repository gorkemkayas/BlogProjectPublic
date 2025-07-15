using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Application.DTOs
{
    public class SelectItemDto
    {
        public string? Text { get; set; }
        public string? Value { get; set; }
        public bool Selected { get; set; }
    }
}
