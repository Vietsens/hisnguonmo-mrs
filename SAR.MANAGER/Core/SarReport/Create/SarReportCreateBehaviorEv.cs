using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using SAR.MANAGER.Core.Check;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Create
{
    class SarReportCreateBehaviorEv : BeanObjectBase, ISarReportCreate
    {
        SAR_REPORT entity;

        internal SarReportCreateBehaviorEv(CommonParam param, SAR_REPORT data)
            : base(param)
        {
            entity = data;
        }

        bool ISarReportCreate.Run()
        {
            bool result = false;
            try
            {
                result = Check() && DAOWorker.SarReportDAO.Create(entity);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }

        bool Check()
        {
            bool result = true;
            try
            {
                result = result && SarReportCheckVerifyValidData.Verify(param, entity);
                result = result && SarReportCheckVerifyExistsCode.Verify(param, entity.REPORT_CODE, null);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
