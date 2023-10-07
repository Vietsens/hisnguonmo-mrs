using HTC.DAO.StagingObject;
using HTC.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.DAO.HtcRevenue
{
    public partial class HtcRevenueDAO : EntityBase
    {
        private HtcRevenueGet GetWorker
        {
            get
            {
                return (HtcRevenueGet)Worker.Get<HtcRevenueGet>();
            }
        }

        public List<HTC_REVENUE> Get(HtcRevenueSO search, CommonParam param)
        {
            List<HTC_REVENUE> result = new List<HTC_REVENUE>();
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

        public HTC_REVENUE GetById(long id, HtcRevenueSO search)
        {
            HTC_REVENUE result = null;
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
