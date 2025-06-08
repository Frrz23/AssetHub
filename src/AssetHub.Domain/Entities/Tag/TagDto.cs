using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Entities.Tag
{
    public class TagDto
    {
        public Guid Id { get; set; }

        public string MacAddress { get; set; }

        public bool IsActive { get; set; }
    }
}
