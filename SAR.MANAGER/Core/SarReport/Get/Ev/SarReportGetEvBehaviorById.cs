using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Get.Ev
{
    class SarReportGetEvBehaviorById : BeanObjectBase, ISarReportGetEv
    {
        long id;

        internal SarReportGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SAR_REPORT ISarReportGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportDAO.GetById(id, new SarReportFilterQuery().Query());
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
