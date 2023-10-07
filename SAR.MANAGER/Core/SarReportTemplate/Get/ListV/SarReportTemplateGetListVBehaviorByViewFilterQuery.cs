using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.ListV
{
    class SarReportTemplateGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISarReportTemplateGetListV
    {
        SarReportTemplateViewFilterQuery filterQuery;

        internal SarReportTemplateGetListVBehaviorByViewFilterQuery(CommonParam param, SarReportTemplateViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SAR_REPORT_TEMPLATE> ISarReportTemplateGetListV.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.GetView(filterQuery.Query(), param);
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
