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
        internal List<V_HIS_MEDICINE_2> GetView2ByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisMedicineView2FilterQuery filter = new HisMedicineView2FilterQuery();
                filter.IDs = ids;
                return this.GetView2(filter);
            }
            return null;
        }

        internal List<V_HIS_MEDICINE_2> GetView2(HisMedicineView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisMedicineView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_2 GetView2ById(long id, HisMedicineView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineDAO.GetView2ById(id, filter.Query());
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
