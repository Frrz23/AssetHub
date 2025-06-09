using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Application.Contracts.Tags
{
    public class CreateTagDto
    {
        [Required]
        public string MacAddress { get; set; }
    }
}
