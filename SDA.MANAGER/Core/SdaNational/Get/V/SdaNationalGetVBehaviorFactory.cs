using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.V
{
    class SdaNationalGetVBehaviorFactory
    {
        internal static ISdaNationalGetV MakeISdaNationalGetV(CommonParam param, object data)
        {
            ISdaNationalGetV result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SdaNationalGetVBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SdaNationalGetVBehaviorById(param, long.Parse(data.ToString()));
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
