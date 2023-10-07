using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMedicine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisImpMestMedicine
{
    partial class HisImpMestMedicineGet : GetBase
    {
        internal List<V_HIS_IMP_MEST_MEDICINE_2> GetView2(HisImpMestMedicineView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisImpMestMedicineView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE_2 GetView2ById(long id, HisImpMestMedicineView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetView2ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE_2> GetView2ByImpMestId(long id)
        {
            try
            {
                HisImpMestMedicineView2FilterQuery filter = new HisImpMestMedicineView2FilterQuery();
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

        internal List<V_HIS_IMP_MEST_MEDICINE_2> GetView2ByImpMestIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisImpMestMedicineView2FilterQuery filter = new HisImpMestMedicineView2FilterQuery();
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
