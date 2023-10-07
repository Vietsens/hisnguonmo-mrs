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
        internal V_HIS_SERVICE_REQ_7 GetView7ById(long id)
        {
            try
            {
                return GetView7ById(id, new HisServiceReqView7FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ_7 GetView7ById(long id, HisServiceReqView7FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView7ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_7> GetView7(HisServiceReqView7FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView7(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_7> GetView7ByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqView7FilterQuery filter = new HisServiceReqView7FilterQuery();
                    filter.IDs = ids;
                    return this.GetView7(filter);
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

        internal List<V_HIS_SERVICE_REQ_7> GetView7ByTreatmentId(long id)
        {
            try
            {
                HisServiceReqView7FilterQuery filter = new HisServiceReqView7FilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView7(filter);
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
