using AgencyCaseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Application.DTOs
{
    public class CaseDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CaseStatus Status { get; set; } = CaseStatus.Open;
        public DateTime OpenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        //FKs
        //public Guid ClientId { get; set; }
        public Guid? AssignedUserId { get; set; }
    }
}
