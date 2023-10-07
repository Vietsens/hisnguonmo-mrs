using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisSereServReha;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrain
{
    class HisRehaTrainGet : GetBase
    {
        internal HisRehaTrainGet()
            : base()
        {

        }

        internal HisRehaTrainGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REHA_TRAIN> Get(HisRehaTrainFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_REHA_TRAIN> GetView(HisRehaTrainViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_REHA_TRAIN> GetViewBySereServRehaIds(List<long> sereServRehaIds)
        {
            try
            {
                if (sereServRehaIds != null)
                {
                    HisRehaTrainViewFilterQuery filter = new HisRehaTrainViewFilterQuery();
                    filter.SERE_SERV_REHA_IDs = sereServRehaIds;
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

        internal List<V_HIS_REHA_TRAIN> GetViewByRehaSumId(long rehaSumId)
        {
            try
            {
                try
                {
                    List<V_HIS_REHA_TRAIN> result = null;
                    List<V_HIS_SERE_SERV_REHA> sereServRehas = new HisSereServRehaGet().GetViewByRehaSumId(rehaSumId);
                    if (IsNotNullOrEmpty(sereServRehas))
                    {
                        result = this.GetViewBySereServRehaIds(sereServRehas.Select(o => o.ID).ToList());
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
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_REHA_TRAIN> GetViewByServiceReqId(long serviceReqId)
        {
            try
            {
                try
                {
                    List<V_HIS_REHA_TRAIN> result = null;
                    List<V_HIS_SERE_SERV_REHA> sereServRehas = new HisSereServRehaGet().GetViewByServiceReqId(serviceReqId);
                    if (IsNotNullOrEmpty(sereServRehas))
                    {
                        result = this.GetViewBySereServRehaIds(sereServRehas.Select(o => o.ID).ToList());
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
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN GetById(long id)
        {
            try
            {
                return GetById(id, new HisRehaTrainFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_TRAIN GetById(long id, HisRehaTrainFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaTrainDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REHA_TRAIN> GetBySereServRehaId(long id)
        {
            try
            {
                HisRehaTrainFilterQuery filter = new HisRehaTrainFilterQuery();
                filter.SERE_SERV_REHA_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_REHA_TRAIN> GetBySereServRehaIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisRehaTrainFilterQuery filter = new HisRehaTrainFilterQuery();
                    filter.SERE_SERV_REHA_IDs = ids;
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
    }
}
