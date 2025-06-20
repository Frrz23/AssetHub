using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.AssetAssignments
{
    public class AssetAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }

}
