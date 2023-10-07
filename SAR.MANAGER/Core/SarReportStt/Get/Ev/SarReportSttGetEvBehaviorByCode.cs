using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportStt.Get.Ev
{
    class SarReportSttGetEvBehaviorByCode : BeanObjectBase, ISarReportSttGetEv
    {
        string code;

        internal SarReportSttGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SAR_REPORT_STT ISarReportSttGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportSttDAO.GetByCode(code, new SarReportSttFilterQuery().Query());
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
