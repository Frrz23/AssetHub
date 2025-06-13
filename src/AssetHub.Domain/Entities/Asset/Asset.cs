using AssetHub.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace AssetHub.Entities.Asset
{
    [Index(nameof(SerialNumber), IsUnique = true)]
    public class Asset : AuditedAggregateRoot<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string AssetName { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string Department { get; set; }

        public DateTime ReceivedDate { get; set; }

        public Guid? TagId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public Guid? RequestedById { get; set; }
        public Guid? ApprovedById { get; set; }
        public DateTime? ApprovedTime { get; set; }

    }

}

