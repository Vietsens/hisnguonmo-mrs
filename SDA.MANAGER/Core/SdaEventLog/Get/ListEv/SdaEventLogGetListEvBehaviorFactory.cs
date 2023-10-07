using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEventLog.Get.ListEv
{
    class SdaEventLogGetListEvBehaviorFactory
    {
        internal static ISdaEventLogGetListEv MakeISdaEventLogGetListEv(CommonParam param, object data)
        {
            ISdaEventLogGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaEventLogFilterQuery))
                {
                    result = new SdaEventLogGetListEvBehaviorByFilterQuery(param, (SdaEventLogFilterQuery)data);
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
