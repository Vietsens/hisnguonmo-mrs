using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport.Get.ListV
{
    class SarReportGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISarReportGetListV
    {
        SarReportViewFilterQuery filterQuery;

        internal SarReportGetListVBehaviorByViewFilterQuery(CommonParam param, SarReportViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SAR_REPORT> ISarReportGetListV.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.GetView(filterQuery.Query(), param);
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
