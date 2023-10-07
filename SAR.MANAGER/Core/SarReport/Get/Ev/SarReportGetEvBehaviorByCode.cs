using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.Ev
{
    class SarReportGetEvBehaviorByCode : BeanObjectBase, ISarReportGetEv
    {
        string code;

        internal SarReportGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SAR_REPORT ISarReportGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.GetByCode(code, new SarReportFilterQuery().Query());
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
