using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Entities.Asset
{
    public class AssetDto
    {
        public Guid Id { get; set; }

        public string AssetName { get; set; }

        public string SerialNumber { get; set; }

        public string Category { get; set; }

        public string Department { get; set; }

        public DateTime ReceivedDate { get; set; }

        public Guid? TagId { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime ApprovedTime { get; internal set; }
    }
}
