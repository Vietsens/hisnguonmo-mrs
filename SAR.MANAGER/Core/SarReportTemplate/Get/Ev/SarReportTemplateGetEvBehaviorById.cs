using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportTemplate.Get.Ev
{
    class SarReportTemplateGetEvBehaviorById : BeanObjectBase, ISarReportTemplateGetEv
    {
        long id;

        internal SarReportTemplateGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SAR_REPORT_TEMPLATE ISarReportTemplateGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTemplateDAO.GetById(id, new SarReportTemplateFilterQuery().Query());
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
