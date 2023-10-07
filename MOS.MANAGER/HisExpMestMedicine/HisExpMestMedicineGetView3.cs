using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineBean;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisExpMestMedicine
{
    partial class HisExpMestMedicineGet : GetBase
    {
        internal List<V_HIS_EXP_MEST_MEDICINE_3> GetView3(HisExpMestMedicineView3FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MEDICINE_3> result = DAOWorker.HisExpMestMedicineDAO.GetView3(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_3 GetView3ById(long id)
        {
            try
            {
                return GetView3ById(id, new HisExpMestMedicineView3FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_3 GetView3ById(long id, HisExpMestMedicineView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.GetView3ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMedicineView3FilterQuery filter = new HisExpMestMedicineView3FilterQuery();
                filter.IDs = ids;
                return this.GetView3(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestMedicineView3FilterQuery filter = new HisExpMestMedicineView3FilterQuery();
                    filter.EXP_MEST_IDs = expMestIds;
                    return this.GetView3(filter);
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

        internal List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByExpMestId(long expMestId)
        {
            HisExpMestMedicineView3FilterQuery filter = new HisExpMestMedicineView3FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView3(filter);
        }
    }
}
