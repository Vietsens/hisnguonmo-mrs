using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCare
{
    partial class HisCareGet : BusinessBase
    {
        internal HisCareGet()
            : base()
        {

        }

        internal HisCareGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARE> Get(HisCareFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE> GetByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisCareFilterQuery filter = new HisCareFilterQuery();
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

        internal HIS_CARE GetById(long id)
        {
            try
            {
                return GetById(id, new HisCareFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE GetById(long id, HisCareFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE> GetByTreatmentId(long id)
        {
            try
            {
                HisCareFilterQuery filter = new HisCareFilterQuery();
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

        internal List<HIS_CARE> GetByAwarenessId(long id)
        {
            try
            {
                HisCareFilterQuery filter = new HisCareFilterQuery();
                filter.AWARENESS_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE> GetByCareSumId(long careSumId)
        {
            try
            {
                HisCareFilterQuery filter = new HisCareFilterQuery();
                filter.CARE_SUM_ID = careSumId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE> GetByDhstId(long dhstId)
        {
            try
            {
                HisCareFilterQuery filter = new HisCareFilterQuery();
                filter.DHST_ID = dhstId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARE> GetByTrackingId(long trackingId)
        {
            try
            {
                HisCareFilterQuery filter = new HisCareFilterQuery();
                filter.TRACKING_ID = trackingId;
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
