using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Application.Results
{
    public class BeginActivationResult
    {
        public bool Succeeded { get; set; }
        public bool RequiresPasswordSetup { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
