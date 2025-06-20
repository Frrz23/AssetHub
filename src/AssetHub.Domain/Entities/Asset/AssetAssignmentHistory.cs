using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace AssetHub.Entities.Asset
{
    public class AssetAssignmentHistory : AuditedAggregateRoot<Guid>
    {
        public Guid AssetId { get; set; }
        public string AssignedTo { get; set; } = default!;
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public AssetAssignmentHistory() { }

        public AssetAssignmentHistory(Guid assetId, string assignedTo, DateTime assignedDate)
        {
            AssetId = assetId;
            AssignedTo = assignedTo;
            AssignedDate = assignedDate;
        }
    }

}
