using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcPeriodDepartment
{
    public partial class HtcPeriodDepartmentDAO : EntityBase
    {
        private HtcPeriodDepartmentGet GetWorker
        {
            get
            {
                return (HtcPeriodDepartmentGet)Worker.Get<HtcPeriodDepartmentGet>();
            }
        }

        public List<HTC_PERIOD_DEPARTMENT> Get(HtcPeriodDepartmentSO search, CommonParam param)
        {
            List<HTC_PERIOD_DEPARTMENT> result = new List<HTC_PERIOD_DEPARTMENT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HTC_PERIOD_DEPARTMENT GetById(long id, HtcPeriodDepartmentSO search)
        {
            HTC_PERIOD_DEPARTMENT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public List<V_HTC_PERIOD_DEPARTMENT> GetView(HtcPeriodDepartmentSO search, CommonParam param)
        {
            List<V_HTC_PERIOD_DEPARTMENT> result = new List<V_HTC_PERIOD_DEPARTMENT>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HTC_PERIOD_DEPARTMENT GetViewById(long id, HtcPeriodDepartmentSO search)
        {
            V_HTC_PERIOD_DEPARTMENT result = null;
            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
