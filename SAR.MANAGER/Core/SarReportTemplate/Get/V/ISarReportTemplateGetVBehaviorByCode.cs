using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.V
{
    class SarReportTemplateGetVBehaviorByCode : BeanObjectBase, ISarReportTemplateGetV
    {
        string code;

        internal SarReportTemplateGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SAR_REPORT_TEMPLATE ISarReportTemplateGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.GetViewByCode(code, new SarReportTemplateViewFilterQuery().Query());
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
