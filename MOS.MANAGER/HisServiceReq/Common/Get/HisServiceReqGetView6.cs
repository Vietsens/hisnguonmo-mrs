using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisServiceReq
{
    partial class HisServiceReqGet : GetBase
    {
        internal V_HIS_SERVICE_REQ_6 GetView6ById(long id)
        {
            try
            {
                return GetView6ById(id, new HisServiceReqView6FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ_6 GetView6ById(long id, HisServiceReqView6FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView6ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_6> GetView6(HisServiceReqView6FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView6(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_6> GetView6ByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqView6FilterQuery filter = new HisServiceReqView6FilterQuery();
                    filter.IDs = ids;
                    return this.GetView6(filter);
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

        internal List<V_HIS_SERVICE_REQ_6> GetView6ByTreatmentId(long id)
        {
            try
            {
                HisServiceReqView6FilterQuery filter = new HisServiceReqView6FilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView6(filter);
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
