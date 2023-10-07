using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMest.Common.Get;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisServiceReq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisExpMestMaterial
{
    partial class HisExpMestMaterialGet : GetBase
    {
        internal HisExpMestMaterialGet()
            : base()
        {

        }

        internal HisExpMestMaterialGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_MATERIAL> Get(HisExpMestMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
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

        internal List<HIS_EXP_MEST_MATERIAL> GetByExpMestId(long expMestId)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_ID = expMestId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetExportedByExpMestId(long expMestId)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_ID = expMestId;
                filter.IS_EXPORT = true;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetExportedByExpMestIds(List<long> expMestIds)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                filter.IS_EXPORT = true;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetUnexportByExpMestId(long expMestId)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_ID = expMestId;
                filter.IS_EXPORT = false;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetRequestByExpMestId(long expMestId)
        {
            HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetExecuteByExpMestId(long expMestId)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_ID = expMestId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetByMaterialId(long id)
        {
            try
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.MATERIAL_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST_MATERIAL> GetByExpMestIds(List<long> expMestIds)
        {
            if (expMestIds != null)
            {
                HisExpMestMaterialFilterQuery filter = new HisExpMestMaterialFilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.Get(filter);
            }
            return null;
        }

        internal HIS_EXP_MEST_MATERIAL GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestMaterialFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_MATERIAL GetById(long id, HisExpMestMaterialFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATERIAL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestMaterialViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATERIAL GetViewById(long id, HisExpMestMaterialViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMaterialDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewByTreatmentId(long treatmentId)
        {
            try
            {
                List<V_HIS_EXP_MEST_MATERIAL> result = null;
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

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewByExpMestIds(List<long> expMestIds)
        {
            try
            {
                if (expMestIds != null)
                {
                    HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
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

        internal List<V_HIS_EXP_MEST_MATERIAL> GetView(HisExpMestMaterialViewFilterQuery filter)
        {
            try
            {
                List<V_HIS_EXP_MEST_MATERIAL> result = DAOWorker.HisExpMestMaterialDAO.GetView(filter.Query(), param);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
                    filter.IDs = ids;
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

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewByExpMestId(long expMestId)
        {
            try
            {
                HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
                filter.EXP_MEST_ID = expMestId;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewRequestByExpMestId(long expMestId)
        {
            HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewExecuteByExpMestId(long expMestId)
        {
            HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView(filter);
        }

        internal List<V_HIS_EXP_MEST_MATERIAL> GetViewByAggrExpMestId(long aggrExpMestId)
        {
            try
            {
                HisExpMestMaterialViewFilterQuery filter = new HisExpMestMaterialViewFilterQuery();
                filter.AGGR_EXP_MEST_ID = aggrExpMestId;
                return this.GetView(filter);
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
