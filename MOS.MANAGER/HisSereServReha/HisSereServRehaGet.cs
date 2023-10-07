using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServReha
{
    partial class HisSereServRehaGet : BusinessBase
    {
        internal HisSereServRehaGet()
            : base()
        {

        }

        internal HisSereServRehaGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_REHA> Get(HisSereServRehaFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServRehaDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERE_SERV_REHA> GetView(HisSereServRehaViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServRehaDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERE_SERV_REHA> GetViewByRehaSumId(long rehaSumId)
        {
            try
            {
                List<V_HIS_SERE_SERV_REHA> result = null;
                List<HIS_SERVICE_REQ> serviceReqs = new HisServiceReqGet().GetByRehaSumId(rehaSumId);
                if (IsNotNullOrEmpty(serviceReqs))
                {
                    result = this.GetViewByServiceReqIds(serviceReqs.Select(o => o.ID).ToList());
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_REHA> GetByRehaTrainTypeId(long rehaTrainTypeId)
        {
            try
            {
                HisSereServRehaFilterQuery filter = new HisSereServRehaFilterQuery();
                filter.REHA_TRAIN_TYPE_ID = rehaTrainTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_REHA> GetBySereServIdAndRehaTrainTypeId(long sereServId, long rehaTrainTypeId)
        {
            HisSereServRehaFilterQuery filter = new HisSereServRehaFilterQuery();
            filter.REHA_TRAIN_TYPE_ID = rehaTrainTypeId;
            filter.SERE_SERV_ID = sereServId;
            return this.Get(filter);
        }

        internal HIS_SERE_SERV_REHA GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServRehaFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_REHA GetById(long id, HisSereServRehaFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServRehaDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal List<HIS_SERE_SERV_REHA> GetBySereServIds(List<long> sereServIds)
        {
            if (IsNotNullOrEmpty(sereServIds))
            {
                HisSereServRehaFilterQuery filter = new HisSereServRehaFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_SERE_SERV_REHA> GetBySereServId(long sereServId)
        {
            HisSereServRehaFilterQuery filter = new HisSereServRehaFilterQuery();
            filter.SERE_SERV_ID = sereServId;
            return this.Get(filter);
        }

        internal List<V_HIS_SERE_SERV_REHA> GetViewByServiceReqIds(List<long> serviceReqIds)
        {
            if (IsNotNullOrEmpty(serviceReqIds))
            {
                HisSereServRehaViewFilterQuery filter = new HisSereServRehaViewFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<V_HIS_SERE_SERV_REHA> GetViewByServiceReqId(long serviceReqId)
        {
            HisSereServRehaViewFilterQuery filter = new HisSereServRehaViewFilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            return this.GetView(filter);
        }
    }
}
