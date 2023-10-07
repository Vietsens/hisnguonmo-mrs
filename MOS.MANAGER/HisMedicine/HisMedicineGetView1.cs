using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMedicineType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisMedicine
{
    partial class HisMedicineGet : GetBase
    {
        internal List<V_HIS_MEDICINE_1> GetView1ByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisMedicineView1FilterQuery filter = new HisMedicineView1FilterQuery();
                filter.IDs = ids;
                return this.GetView1(filter);
            }
            return null;
        }

        internal List<V_HIS_MEDICINE_1> GetView1(HisMedicineView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisMedicineView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_1 GetView1ById(long id, HisMedicineView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetView1ById(id, filter.Query());
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
