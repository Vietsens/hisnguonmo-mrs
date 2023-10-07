using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.V
{
    class SarReportTemplateGetVBehaviorById : BeanObjectBase, ISarReportTemplateGetV
    {
        long id;

        internal SarReportTemplateGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SAR_REPORT_TEMPLATE ISarReportTemplateGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.GetViewById(id, new SarReportTemplateViewFilterQuery().Query());
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
