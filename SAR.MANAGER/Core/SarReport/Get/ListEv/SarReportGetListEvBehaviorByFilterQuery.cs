using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport.Get.ListEv
{
    class SarReportGetListEvBehaviorByFilterQuery : BeanObjectBase, ISarReportGetListEv
    {
        SarReportFilterQuery filterQuery;

        internal SarReportGetListEvBehaviorByFilterQuery(CommonParam param, SarReportFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SAR_REPORT> ISarReportGetListEv.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.Get(filterQuery.Query(), param);
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
