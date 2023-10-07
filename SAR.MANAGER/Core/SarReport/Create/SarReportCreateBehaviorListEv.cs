using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Base;
using SAR.MANAGER.Core.Check;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport.Create
{
    class SarReportCreateBehaviorListEv : BeanObjectBase, ISarReportCreate
    {
        List<SAR_REPORT> entities;

        internal SarReportCreateBehaviorListEv(CommonParam param, List<SAR_REPORT> datas)
            : base(param)
        {
            entities = datas;
        }

        bool ISarReportCreate.Run()
        {
            bool result = false;
            try
            {
                result = Check() && DAOWorker.SarReportDAO.CreateList(entities);
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
                result = result && SarReportCheckVerifyValidData.Verify(param, entities);
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
