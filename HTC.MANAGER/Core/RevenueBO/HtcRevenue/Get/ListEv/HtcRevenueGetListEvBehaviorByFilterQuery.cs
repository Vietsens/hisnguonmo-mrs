using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.ListEv
{
    class HtcRevenueGetListEvBehaviorByFilterQuery : BeanObjectBase, IHtcRevenueGetListEv
    {
        HtcRevenueFilterQuery filterQuery;

        internal HtcRevenueGetListEvBehaviorByFilterQuery(CommonParam param, HtcRevenueFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<HTC_REVENUE> IHtcRevenueGetListEv.Run()
        {
            try
            {
                return DAOWorker.HtcRevenueDAO.Get(filterQuery.Query(), param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
