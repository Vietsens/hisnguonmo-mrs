using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMaterialBean;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisExpMestMaterial
{
    partial class HisExpMestMaterialGet : GetBase
    {
        internal List<V_HIS_EXP_MEST_MATERIAL_4> GetView4(HisExpMestMaterialView4FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MATERIAL_4> result = DAOWorker.HisExpMestMaterialDAO.GetView4(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMaterialView4FilterQuery filter = new HisExpMestMaterialView4FilterQuery();
                filter.IDs = ids;
                return this.GetView4(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByExpMestId(long expMestId)
        {
            HisExpMestMaterialView4FilterQuery filter = new HisExpMestMaterialView4FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView4(filter);
        }

        internal V_HIS_EXP_MEST_MATERIAL_4 GetView4ById(long id)
        {
            try
            {
                return GetView4ById(id, new HisExpMestMaterialView4FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATERIAL_4 GetView4ById(long id, HisExpMestMaterialView4FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.GetView4ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByExpMestIds(List<long> expMestIds)
        {
            if (expMestIds != null)
            {
                HisExpMestMaterialView4FilterQuery filter = new HisExpMestMaterialView4FilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.GetView4(filter);
            }
            return null;
        }
    }
}
