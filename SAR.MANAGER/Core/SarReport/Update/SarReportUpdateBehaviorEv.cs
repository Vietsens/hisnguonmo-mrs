using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using SAR.MANAGER.Core.Check;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Update
{
    class SarReportUpdateBehaviorEv : BeanObjectBase, ISarReportUpdate
    {
        SAR_REPORT entity;

        internal SarReportUpdateBehaviorEv(CommonParam param, SAR_REPORT data)
            : base(param)
        {
            entity = data;
        }

        bool ISarReportUpdate.Run()
        {
            bool result = false;
            try
            {
                result = Check() && DAOWorker.SarReportDAO.Update(entity);
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
                result = result && SarReportCheckVerifyIsUnlock.Verify(param, entity.ID);
                result = result && SarReportCheckVerifyExistsCode.Verify(param, entity.REPORT_CODE, entity.ID);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = false;
            }
            return result;
        }
    }
}
