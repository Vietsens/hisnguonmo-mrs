using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.V
{
    class SarReportGetVBehaviorByCode : BeanObjectBase, ISarReportGetV
    {
        string code;

        internal SarReportGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SAR_REPORT ISarReportGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.GetViewByCode(code, new SarReportViewFilterQuery().Query());
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
