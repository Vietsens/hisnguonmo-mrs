using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.V
{
    class SarReportGetVBehaviorById : BeanObjectBase, ISarReportGetV
    {
        long id;

        internal SarReportGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SAR_REPORT ISarReportGetV.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.GetViewById(id, new SarReportViewFilterQuery().Query());
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
