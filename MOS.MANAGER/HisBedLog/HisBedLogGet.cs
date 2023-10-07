using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedLog
{
    partial class HisBedLogGet : BusinessBase
    {
        internal HisBedLogGet()
            : base()
        {

        }

        internal HisBedLogGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BED_LOG> Get(HisBedLogFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedLogDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_LOG GetById(long id)
        {
            try
            {
                return GetById(id, new HisBedLogFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_LOG GetById(long id, HisBedLogFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedLogDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BED_LOG> GetByBedId(long id)
        {
            HisBedLogFilterQuery filter = new HisBedLogFilterQuery();
            filter.BED_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_BED_LOG> GetByTreatmentBedRoomId(long id)
        {
            HisBedLogFilterQuery filter = new HisBedLogFilterQuery();
            filter.TREATMENT_BED_ROOM_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_BED_LOG> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisBedLogFilterQuery filter = new HisBedLogFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_BED_LOG> GetByServiceReqId(long serviceReqId)
        {
            HisBedLogFilterQuery filter = new HisBedLogFilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            return this.Get(filter);
        }
    }
}
