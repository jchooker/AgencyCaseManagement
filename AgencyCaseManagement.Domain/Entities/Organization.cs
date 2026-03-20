using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyCaseManagement.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public OrganizationType OrganizationType { get; set; } = OrganizationType.Government;
    }

    public enum OrganizationType { Nonprofit, Government, Healthcare, Legal}
}
