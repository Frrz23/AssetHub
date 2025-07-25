using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Application.Contracts.Tags
{
    public class TagDto
    {
        public Guid Id { get; set; }

        public string MACAddress { get; set; }

        public bool IsActive { get; set; }
    }
}
