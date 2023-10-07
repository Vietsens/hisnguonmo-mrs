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
        internal List<V_HIS_EXP_MEST_MEDICINE_1> GetView1(HisExpMestMedicineView1FilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MEDICINE_1> result = DAOWorker.HisExpMestMedicineDAO.GetView1(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisExpMestMedicineView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE_1 GetView1ById(long id, HisExpMestMedicineView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_1> GetView1ByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMedicineView1FilterQuery filter = new HisExpMestMedicineView1FilterQuery();
                filter.IDs = ids;
                return this.GetView1(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MEDICINE_1> GetView1ByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestMedicineView1FilterQuery filter = new HisExpMestMedicineView1FilterQuery();
                    filter.EXP_MEST_IDs = expMestIds;
                    return this.GetView1(filter);
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

        internal List<V_HIS_EXP_MEST_MEDICINE_1> GetView1ByExpMestId(long expMestId)
        {
            HisExpMestMedicineView1FilterQuery filter = new HisExpMestMedicineView1FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView1(filter);
        }
    }
}
