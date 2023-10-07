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
        internal List<V_HIS_IMP_MEST_MEDICINE_3> GetView3(HisImpMestMedicineView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetView3(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE_3 GetView3ById(long id)
        {
            try
            {
                return GetView3ById(id, new HisImpMestMedicineView3FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MEDICINE_3 GetView3ById(long id, HisImpMestMedicineView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestMedicineDAO.GetView3ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE_3> GetView3ByImpMestId(long id)
        {
            try
            {
                HisImpMestMedicineView3FilterQuery filter = new HisImpMestMedicineView3FilterQuery();
                filter.IMP_MEST_ID = id;
                return this.GetView3(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST_MEDICINE_3> GetView3ByImpMestIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisImpMestMedicineView3FilterQuery filter = new HisImpMestMedicineView3FilterQuery();
                    filter.IMP_MEST_IDs = ids;
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
    }
}
