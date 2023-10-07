using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportStt.Get.Ev
{
    class SarReportSttGetEvBehaviorById : BeanObjectBase, ISarReportSttGetEv
    {
        long id;

        internal SarReportSttGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SAR_REPORT_STT ISarReportSttGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportSttDAO.GetById(id, new SarReportSttFilterQuery().Query());
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
