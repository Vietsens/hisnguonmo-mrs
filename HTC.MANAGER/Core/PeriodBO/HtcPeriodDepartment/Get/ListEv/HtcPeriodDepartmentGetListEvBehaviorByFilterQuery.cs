using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace HTC.MANAGER.Core.PeriodBO.HtcPeriodDepartment.Get.ListEv
{
    class HtcPeriodDepartmentGetListEvBehaviorByFilterQuery : BeanObjectBase, IHtcPeriodDepartmentGetListEv
    {
        HtcPeriodDepartmentFilterQuery filterQuery;

        internal HtcPeriodDepartmentGetListEvBehaviorByFilterQuery(CommonParam param, HtcPeriodDepartmentFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<HTC_PERIOD_DEPARTMENT> IHtcPeriodDepartmentGetListEv.Run()
        {
            try
            {
                return DAOWorker.HtcPeriodDepartmentDAO.Get(filterQuery.Query(), param);
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
