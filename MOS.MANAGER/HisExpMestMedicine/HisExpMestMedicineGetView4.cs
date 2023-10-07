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
        internal List<V_HIS_EXP_MEST_MEDICINE_4> GetView4(HisExpMestMedicineView4FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MEDICINE_4> result = DAOWorker.HisExpMestMedicineDAO.GetView4(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_4 GetView4ById(long id)
        {
            try
            {
                return GetView4ById(id, new HisExpMestMedicineView4FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_4 GetView4ById(long id, HisExpMestMedicineView4FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.GetView4ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_4> GetView4ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMedicineView4FilterQuery filter = new HisExpMestMedicineView4FilterQuery();
                filter.IDs = ids;
                return this.GetView4(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_4> GetView4ByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestMedicineView4FilterQuery filter = new HisExpMestMedicineView4FilterQuery();
                    filter.EXP_MEST_IDs = expMestIds;
                    return this.GetView4(filter);
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

        internal List<V_HIS_EXP_MEST_MEDICINE_4> GetView4ByExpMestId(long expMestId)
        {
            HisExpMestMedicineView4FilterQuery filter = new HisExpMestMedicineView4FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView4(filter);
        }
    }
}
