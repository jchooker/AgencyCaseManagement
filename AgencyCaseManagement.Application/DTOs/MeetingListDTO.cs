using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AgencyCaseManagement.Application.DTOs
{
    public class MeetingListDTO
    {
        [Required]
        public List<MeetingDTO> Meetings { get; set; } = null!;
    }
}
