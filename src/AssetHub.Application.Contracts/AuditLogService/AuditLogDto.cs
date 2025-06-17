using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.AuditLogService
{
    public class AuditLogDto
    {
        public string EntityName { get; set; }
        public string Action { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string Description { get; set; }
    }

}
