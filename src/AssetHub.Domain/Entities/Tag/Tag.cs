using AssetHub.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace AssetHub.Entities.Tag
{
    public class Tag : Entity<Guid>
    {
        public string MACAddress { get; set; }
        public bool IsActive { get; set; } = true;

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
        public Guid? RequestedBy { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
