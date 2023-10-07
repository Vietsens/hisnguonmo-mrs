using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTracking
{
    partial class HisTrackingGet : BusinessBase
    {
        internal HisTrackingGet()
            : base()
        {

        }

        internal HisTrackingGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TRACKING> Get(HisTrackingFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTrackingDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TRACKING> GetView(HisTrackingViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTrackingDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRACKING GetById(long id)
        {
            try
            {
                return GetById(id, new HisTrackingFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRACKING> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisTrackingFilterQuery filter = new HisTrackingFilterQuery();
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

        internal List<V_HIS_TRACKING> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisTrackingViewFilterQuery filter = new HisTrackingViewFilterQuery();
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

        internal HIS_TRACKING GetById(long id, HisTrackingFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTrackingDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_TRACKING GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTrackingViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRACKING GetViewById(long id, HisTrackingViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTrackingDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRACKING> GetByTreatmentId(long id)
        {
            try
            {
                HisTrackingFilterQuery filter = new HisTrackingFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
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
