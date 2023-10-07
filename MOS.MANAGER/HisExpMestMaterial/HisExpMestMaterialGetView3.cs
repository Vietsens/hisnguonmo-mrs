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
        internal List<V_HIS_EXP_MEST_MATERIAL_3> GetView3(HisExpMestMaterialView3FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MATERIAL_3> result = DAOWorker.HisExpMestMaterialDAO.GetView3(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_3> GetView3ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMaterialView3FilterQuery filter = new HisExpMestMaterialView3FilterQuery();
                filter.IDs = ids;
                return this.GetView3(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_3> GetView3ByExpMestId(long expMestId)
        {
            HisExpMestMaterialView3FilterQuery filter = new HisExpMestMaterialView3FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView3(filter);
        }

        internal V_HIS_EXP_MEST_MATERIAL_3 GetView3ById(long id)
        {
            try
            {
                return GetView3ById(id, new HisExpMestMaterialView3FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATERIAL_3 GetView3ById(long id, HisExpMestMaterialView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.GetView3ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_3> GetView3ByExpMestIds(List<long> expMestIds)
        {
            if (expMestIds != null)
            {
                HisExpMestMaterialView3FilterQuery filter = new HisExpMestMaterialView3FilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.GetView3(filter);
            }
            return null;
        }
    }
}
