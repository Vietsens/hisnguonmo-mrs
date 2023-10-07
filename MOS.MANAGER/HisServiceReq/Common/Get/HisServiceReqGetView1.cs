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
        internal V_HIS_SERVICE_REQ_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisServiceReqView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ_1 GetView1ById(long id, HisServiceReqView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_1> GetView1(HisServiceReqView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ_1> GetView1ByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqView1FilterQuery filter = new HisServiceReqView1FilterQuery();
                    filter.IDs = ids;
                    return this.GetView1(filter);
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

        internal List<V_HIS_SERVICE_REQ_1> GetView1ByTreatmentId(long id)
        {
            try
            {
                HisServiceReqView1FilterQuery filter = new HisServiceReqView1FilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView1(filter);
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
