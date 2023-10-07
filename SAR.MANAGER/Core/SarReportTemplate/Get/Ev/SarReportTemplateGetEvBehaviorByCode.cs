using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.Ev
{
    class SarReportTemplateGetEvBehaviorByCode : BeanObjectBase, ISarReportTemplateGetEv
    {
        string code;

        internal SarReportTemplateGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SAR_REPORT_TEMPLATE ISarReportTemplateGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.GetByCode(code, new SarReportTemplateFilterQuery().Query());
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
