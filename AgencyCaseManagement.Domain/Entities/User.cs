using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AgencyCaseManagement.Domain.Entities
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        //IdentityUser<Guid> already provides Guid Id for BaseEntity
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
