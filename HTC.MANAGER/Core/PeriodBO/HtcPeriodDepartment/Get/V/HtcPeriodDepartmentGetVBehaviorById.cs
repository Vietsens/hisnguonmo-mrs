using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.V
{
    class HtcPeriodDepartmentGetVBehaviorById : BeanObjectBase, IHtcPeriodDepartmentGetV
    {
        long id;

        internal HtcPeriodDepartmentGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_HTC_PERIOD_DEPARTMENT IHtcPeriodDepartmentGetV.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDepartmentDAO.GetViewById(id, new HtcPeriodDepartmentFilterQuery().Query());
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
