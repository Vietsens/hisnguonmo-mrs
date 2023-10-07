using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.UpdateStt
{
    class SarReportUpdateSttBehaviorFactory
    {
        internal static ISarReportUpdateStt MakeISarReportUpdateStt(CommonParam param, object data)
        {
            ISarReportUpdateStt result = null;
            try
            {
                if (data.GetType() == typeof(SAR_REPORT))
                {
                    result = new SarReportUpdateSttBehavior(param, (SAR_REPORT)data);
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
