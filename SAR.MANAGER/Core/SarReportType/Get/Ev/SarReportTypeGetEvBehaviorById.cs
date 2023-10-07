using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportType.Get.Ev
{
    class SarReportTypeGetEvBehaviorById : BeanObjectBase, ISarReportTypeGetEv
    {
        long id;

        internal SarReportTypeGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SAR_REPORT_TYPE ISarReportTypeGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTypeDAO.GetById(id, new SarReportTypeFilterQuery().Query());
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
