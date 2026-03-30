using AgencyCaseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Application.DTOs
{
    public class MeetingDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MeetingType MeetingType { get; set; }
        public string CaseTitle { get; set; }
        public int ParticipantCount { get; set; }
    }
}
