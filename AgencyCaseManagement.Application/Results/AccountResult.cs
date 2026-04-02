using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Application.Results
{
    public class AccountResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
        public string? RedirectAction { get; set; }
    }
}
