using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.Ev
{
    class HtcPeriodDepartmentGetEvBehaviorById : BeanObjectBase, IHtcPeriodDepartmentGetEv
    {
        long id;

        internal HtcPeriodDepartmentGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        HTC_PERIOD_DEPARTMENT IHtcPeriodDepartmentGetEv.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDepartmentDAO.GetById(id, new HtcPeriodDepartmentFilterQuery().Query());
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
