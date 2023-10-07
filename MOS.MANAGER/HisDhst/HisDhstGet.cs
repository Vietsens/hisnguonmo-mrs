using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDhst
{
    class HisDhstGet : GetBase
    {
        internal HisDhstGet()
            : base()
        {

        }

        internal HisDhstGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DHST> Get(HisDhstFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDhstDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DHST GetById(long id)
        {
            try
            {
                return GetById(id, new HisDhstFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DHST GetById(long id, HisDhstFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDhstDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DHST> GetByTreatmentId(long id)
        {
            try
            {
                HisDhstFilterQuery filter = new HisDhstFilterQuery();
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

        internal List<HIS_DHST> GetByTrackingId(long trackingId)
        {
            try
            {
                HisDhstFilterQuery filter = new HisDhstFilterQuery();
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
