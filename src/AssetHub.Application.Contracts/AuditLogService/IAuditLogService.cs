using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.AuditLogService
{
    public interface IAuditLogService
    {
        Task LogAsync(string entityName, string action, string description = null);
    }

}
