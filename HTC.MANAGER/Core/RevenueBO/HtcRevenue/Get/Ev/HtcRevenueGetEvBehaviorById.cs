using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.RevenueBO.HtcRevenue.Get.Ev
{
    class HtcRevenueGetEvBehaviorById : BeanObjectBase, IHtcRevenueGetEv
    {
        long id;

        internal HtcRevenueGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        HTC_REVENUE IHtcRevenueGetEv.Run()
        {
            try
            {
                return DAOWorker.HtcRevenueDAO.GetById(id, new HtcRevenueFilterQuery().Query());
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
