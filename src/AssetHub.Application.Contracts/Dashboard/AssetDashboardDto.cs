using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Dashboard
{
    public class AssetDashboardDto
    {
        public int TotalAssets { get; set; }
        public int ActiveAssets { get; set; }
        public int ApprovedAssets { get; set; }
        public int UnapprovedAssets { get; set; }
        public List<AssetCategoryCountDto> CategoryCounts { get; set; }
        public List<AssetDepartmentCountDto> DepartmentCounts { get; set; }
    }

}
