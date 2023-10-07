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
        internal List<V_HIS_EXP_MEST_MATERIAL_1> GetView1(HisExpMestMaterialView1FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MATERIAL_1> result = DAOWorker.HisExpMestMaterialDAO.GetView1(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_1> GetView1ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMaterialView1FilterQuery filter = new HisExpMestMaterialView1FilterQuery();
                filter.IDs = ids;
                return this.GetView1(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_1> GetView1ByExpMestId(long expMestId)
        {
            HisExpMestMaterialView1FilterQuery filter = new HisExpMestMaterialView1FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView1(filter);
        }

        internal V_HIS_EXP_MEST_MATERIAL_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisExpMestMaterialView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATERIAL_1 GetView1ById(long id, HisExpMestMaterialView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL_1> GetView1ByExpMestIds(List<long> expMestIds)
        {
            if (expMestIds != null)
            {
                HisExpMestMaterialView1FilterQuery filter = new HisExpMestMaterialView1FilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.GetView1(filter);
            }
            return null;
        }
    }
}
