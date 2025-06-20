using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.AssetAssignments
{
    public class CreateAssetAssignmentDto
    {
        public Guid AssetId { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

}
