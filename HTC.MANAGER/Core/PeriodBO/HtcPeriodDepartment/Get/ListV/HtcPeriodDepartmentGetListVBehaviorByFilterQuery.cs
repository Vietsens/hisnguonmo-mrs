using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListV
{
    class HtcPeriodDepartmentGetListVBehaviorByFilterQuery : BeanObjectBase, IHtcPeriodDepartmentGetListV
    {
        HtcPeriodDepartmentViewFilterQuery filterQuery;

        internal HtcPeriodDepartmentGetListVBehaviorByFilterQuery(CommonParam param, HtcPeriodDepartmentViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_HTC_PERIOD_DEPARTMENT> IHtcPeriodDepartmentGetListV.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDepartmentDAO.GetView(filterQuery.Query(), param);
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
