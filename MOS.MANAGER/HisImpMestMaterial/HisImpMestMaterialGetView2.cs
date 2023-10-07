using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMaterial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisImpMestMaterial
{
    partial class HisImpMestMaterialGet : GetBase
    {
        internal List<V_HIS_IMP_MEST_MATERIAL_2> GetView2(HisImpMestMaterialView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MATERIAL_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisImpMestMaterialView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MATERIAL_2 GetView2ById(long id, HisImpMestMaterialView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMaterialDAO.GetView2ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL_2> GetView2ByImpMestId(long id)
        {
            try
            {
                HisImpMestMaterialView2FilterQuery filter = new HisImpMestMaterialView2FilterQuery();
                filter.IMP_MEST_ID = id;
                return this.GetView2(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MATERIAL_2> GetView2ByImpMestIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisImpMestMaterialView2FilterQuery filter = new HisImpMestMaterialView2FilterQuery();
                    filter.IMP_MEST_IDs = ids;
                    return this.GetView2(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
