using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport.Create
{
    class SarReportCreateBehaviorFactory
    {
        internal static ISarReportCreate MakeISarReportCreate(CommonParam param, object data)
        {
            ISarReportCreate result = null;
            try
            {
                if (data.GetType() == typeof(SAR_REPORT))
                {
                    result = new SarReportCreateBehaviorEv(param, (SAR_REPORT)data);
                }
                else if (data.GetType() == typeof(List<SAR_REPORT>))
                {
                    result = new SarReportCreateBehaviorListEv(param, (List<SAR_REPORT>)data);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), ex);
                result = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
