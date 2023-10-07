using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMest.Common.Get;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicineBean;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisExpMestMedicine
{
    partial class HisExpMestMedicineGet : GetBase
    {
        internal HisExpMestMedicineGet()
            : base()
        {

        }

        internal HisExpMestMedicineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetView(HisExpMestMedicineViewFilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MEDICINE> result = DAOWorker.HisExpMestMedicineDAO.GetView(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestMedicineViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MEDICINE GetViewById(long id, HisExpMestMedicineViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewByTreatmentId(long treatmentId)
        {
            try
            {
                List<V_HIS_EXP_MEST_MEDICINE> result = null;
                List<HIS_EXP_MEST> expMests = new HisExpMestGet().GetByTreatmentId(treatmentId);
                if (IsNotNullOrEmpty(expMests))
                {
                    result = this.GetViewByExpMestIds(expMests.Select(o => o.ID).ToList());
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
                    filter.EXP_MEST_IDs = expMestIds;
                    return this.GetView(filter);
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

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewByExpMestId(long expMestId)
        {
            HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewRequestByExpMestId(long expMestId)
        {
            HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_MEDICINE> GetViewByAggrExpMestId(long aggrExpMestId)
        {
            HisExpMestMedicineViewFilterQuery filter = new HisExpMestMedicineViewFilterQuery();
            filter.AGGR_EXP_MEST_ID = aggrExpMestId;
            return this.GetView(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> Get(HisExpMestMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_MEDICINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestMedicineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
                    filter.IDs = ids;
                    return this.Get(filter);
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

        internal List<HIS_EXP_MEST_MEDICINE> GetByExpMestId(long expMestId)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetUnexportByExpMestId(long expMestId)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            filter.IS_EXPORT = false;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetExportedByExpMestId(long expMestId)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            filter.IS_EXPORT = true;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetExportedByExpMestIds(List<long> expMestIds)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.EXP_MEST_IDs = expMestIds;
            filter.IS_EXPORT = true;
            return this.Get(filter);
        }

        internal HIS_EXP_MEST_MEDICINE GetById(long id, HisExpMestMedicineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMedicineDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetByMedicineId(long id)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.MEDICINE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetByMedicineIds(List<long> medicineIds)
        {
            HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
            filter.MEDICINE_IDs = medicineIds;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MEDICINE> GetByExpMestIds(List<long> expMestIds)
        {
            if (expMestIds != null)
            {
                HisExpMestMedicineFilterQuery filter = new HisExpMestMedicineFilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.Get(filter);
            }
            return null;
        }
    }
}
