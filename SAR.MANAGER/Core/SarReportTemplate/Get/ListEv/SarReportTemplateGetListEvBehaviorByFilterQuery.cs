using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.ListEv
{
    class SarReportTemplateGetListEvBehaviorByFilterQuery : BeanObjectBase, ISarReportTemplateGetListEv
    {
        SarReportTemplateFilterQuery filterQuery;

        internal SarReportTemplateGetListEvBehaviorByFilterQuery(CommonParam param, SarReportTemplateFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SAR_REPORT_TEMPLATE> ISarReportTemplateGetListEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.Get(filterQuery.Query(), param);
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
