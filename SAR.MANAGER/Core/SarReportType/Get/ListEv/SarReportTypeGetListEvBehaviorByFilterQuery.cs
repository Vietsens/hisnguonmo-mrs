using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportType.Get.ListEv
{
    class SarReportTypeGetListEvBehaviorByFilterQuery : BeanObjectBase, ISarReportTypeGetListEv
    {
        SarReportTypeFilterQuery filterQuery;

        internal SarReportTypeGetListEvBehaviorByFilterQuery(CommonParam param, SarReportTypeFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SAR_REPORT_TYPE> ISarReportTypeGetListEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTypeDAO.Get(filterQuery.Query(), param);
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
