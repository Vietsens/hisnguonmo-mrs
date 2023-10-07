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
        internal V_HIS_SERVICE_REQ_3 GetView3ById(long id)
        {
            try
            {
                return GetView3ById(id, new HisServiceReqView3FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ_3 GetView3ById(long id, HisServiceReqView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView3ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_3> GetView3(HisServiceReqView3FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView3(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_3> GetView3ByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqView3FilterQuery filter = new HisServiceReqView3FilterQuery();
                    filter.IDs = ids;
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

        internal List<V_HIS_SERVICE_REQ_3> GetView3ByTreatmentId(long id)
        {
            try
            {
                HisServiceReqView3FilterQuery filter = new HisServiceReqView3FilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView3(filter);
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
