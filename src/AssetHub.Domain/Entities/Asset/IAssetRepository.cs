﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace AssetHub.Entities.Asset
{
    public interface IAssetRepository : IRepository<Asset, Guid>
    {
        //  custom methods 
    }
}
