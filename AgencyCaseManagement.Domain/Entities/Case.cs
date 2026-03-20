using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Domain.Entities
{
    public class Case : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CaseStatus Status { get; set; } = CaseStatus.Open;
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        //FKs
        //public Guid ClientId { get; set; }
        public Guid? AssignedUserId { get; set; }
        //public Guid OrganizationId { get; set; }
        //public Organization Organization { get; set; } = null!;

        //public ICollection<CaseTask> Tasks { get; set; } new List<CaseTask>();
        //public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }

    public enum CaseStatus { Open, InProgress, OnHold, Closed}
}
