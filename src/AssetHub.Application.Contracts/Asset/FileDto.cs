using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Asset
{
    public class FileDto
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
