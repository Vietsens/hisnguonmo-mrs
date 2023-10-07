using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.SarReport.Update
{
    class SarReportUpdateBehaviorFactory
    {
        internal static ISarReportUpdate MakeISarReportUpdate(CommonParam param, object data)
        {
            ISarReportUpdate result = null;
            try
            {
                if (data.GetType() == typeof(SAR_REPORT))
                {
                    result = new SarReportUpdateBehaviorEv(param, (SAR_REPORT)data);
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
