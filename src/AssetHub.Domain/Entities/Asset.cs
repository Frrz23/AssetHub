using AssetHub.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AssetHub.Entities
{
    public class Asset : Entity<Guid>
    {
        public string AssetName { get; set; }
        public string SerialNumber { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid? TagId { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        public Guid? RequestedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}

