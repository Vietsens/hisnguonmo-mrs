using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcPeriod
{
    public partial class HtcPeriodDAO : EntityBase
    {
        private HtcPeriodGet GetWorker
        {
            get
            {
                return (HtcPeriodGet)Worker.Get<HtcPeriodGet>();
            }
        }

        public List<HTC_PERIOD> Get(HtcPeriodSO search, CommonParam param)
        {
            List<HTC_PERIOD> result = new List<HTC_PERIOD>();
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

        public HTC_PERIOD GetById(long id, HtcPeriodSO search)
        {
            HTC_PERIOD result = null;
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
    }
}
