using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusion
{
    partial class HisInfusionGet : BusinessBase
    {
        internal HisInfusionGet()
            : base()
        {

        }

        internal HisInfusionGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INFUSION> Get(HisInfusionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_INFUSION> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisInfusionFilterQuery filter = new HisInfusionFilterQuery();
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

        internal List<HIS_INFUSION> GetByInfusionSumId(long infusionSumId)
        {
            HisInfusionFilterQuery filter = new HisInfusionFilterQuery();
            filter.INFUSION_SUM_ID = infusionSumId;
            return this.Get(filter);
        }

        internal List<V_HIS_INFUSION> GetView(HisInfusionViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INFUSION GetById(long id)
        {
            try
            {
                return GetById(id, new HisInfusionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INFUSION GetById(long id, HisInfusionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_INFUSION GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisInfusionViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_INFUSION GetViewById(long id, HisInfusionViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionDAO.GetViewById(id, filter.Query());
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
