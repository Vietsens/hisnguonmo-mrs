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
        internal V_HIS_SERVICE_REQ_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisServiceReqView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ_2 GetView2ById(long id, HisServiceReqView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView2ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_2> GetView2(HisServiceReqView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_2> GetView2ByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqView2FilterQuery filter = new HisServiceReqView2FilterQuery();
                    filter.IDs = ids;
                    return this.GetView2(filter);
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

        internal List<V_HIS_SERVICE_REQ_2> GetView2ByTreatmentId(long id)
        {
            try
            {
                HisServiceReqView2FilterQuery filter = new HisServiceReqView2FilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView2(filter);
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
