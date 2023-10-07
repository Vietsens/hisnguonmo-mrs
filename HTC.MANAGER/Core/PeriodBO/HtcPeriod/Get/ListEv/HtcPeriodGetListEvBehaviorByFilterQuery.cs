using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.ListEv
{
    class HtcPeriodGetListEvBehaviorByFilterQuery : BeanObjectBase, IHtcPeriodGetListEv
    {
        HtcPeriodFilterQuery filterQuery;

        internal HtcPeriodGetListEvBehaviorByFilterQuery(CommonParam param, HtcPeriodFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<HTC_PERIOD> IHtcPeriodGetListEv.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDAO.Get(filterQuery.Query(), param);
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
