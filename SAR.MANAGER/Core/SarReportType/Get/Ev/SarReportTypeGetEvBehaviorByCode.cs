using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReportType.Get.Ev
{
    class SarReportTypeGetEvBehaviorByCode : BeanObjectBase, ISarReportTypeGetEv
    {
        string code;

        internal SarReportTypeGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SAR_REPORT_TYPE ISarReportTypeGetEv.Run()
        {
            try
            {
                return DAOWorker.SarReportTypeDAO.GetByCode(code, new SarReportTypeFilterQuery().Query());
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
