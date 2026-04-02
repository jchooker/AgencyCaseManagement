using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Application.Results
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public bool RequiresActivation { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
