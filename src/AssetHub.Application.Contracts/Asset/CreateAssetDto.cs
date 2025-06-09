using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Entities.Asset
{
    public class CreateAssetDto
    {
        [Required]
        public string AssetName { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Department { get; set; }

        public DateTime ReceivedDate { get; set; }

        public Guid? TagId { get; set; }
    }
}
