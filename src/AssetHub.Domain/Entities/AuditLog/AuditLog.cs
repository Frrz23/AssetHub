using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AssetHub.Entities.AuditLog
{
    public class AuditLog : Entity<Guid>
    {
        public string EntityName { get; set; }
        public string Action { get; set; } // "Create", "Update", "Delete", etc.
        public string PerformedBy { get; set; } // user email or ID
        public DateTime PerformedAt { get; set; } // stored in UTC
        public string Description { get; set; } // optional detailed info

        public AuditLog() { }

        public AuditLog(string entityName, string action, string performedBy, DateTime performedAt, string description)
        {
            EntityName = entityName;
            Action = action;
            PerformedBy = performedBy;
            PerformedAt = performedAt;
            Description = description;
        }
    }
}