using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriod.Get.Ev
{
    class HtcPeriodGetEvBehaviorById : BeanObjectBase, IHtcPeriodGetEv
    {
        long id;

        internal HtcPeriodGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        HTC_PERIOD IHtcPeriodGetEv.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDAO.GetById(id, new HtcPeriodFilterQuery().Query());
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
