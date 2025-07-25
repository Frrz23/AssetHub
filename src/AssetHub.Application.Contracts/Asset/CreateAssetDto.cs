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
    }

}
