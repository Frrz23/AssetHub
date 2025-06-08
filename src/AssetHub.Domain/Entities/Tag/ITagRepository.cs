using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AssetHub.Entities.Tag
{
    public interface ITagRepository : IRepository<Tag, Guid>
    {
        // custom methods 
    }
}
