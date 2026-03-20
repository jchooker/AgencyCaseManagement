using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Domain.Entities
{
    public class Meeting : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MeetingType MeetingType { get; set; } = MeetingType.General;

        //FKs
        public Guid CaseId { get; set; }

        //nav properties
        public Case Case { get; set; } = null!;
        public ICollection<User> Participants { get; set; } = new List<User>();
    }

    public enum MeetingType { General, FamilyInterview, Assessment, Review}
}
