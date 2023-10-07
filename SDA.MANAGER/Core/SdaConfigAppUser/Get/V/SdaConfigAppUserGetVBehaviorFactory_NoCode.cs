using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.V
{
    class SdaConfigAppUserGetVBehaviorFactory
    {
        internal static ISdaConfigAppUserGetV MakeISdaConfigAppUserGetV(CommonParam param, object data)
        {
            ISdaConfigAppUserGetV result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new SdaConfigAppUserGetVBehaviorById(param, long.Parse(data.ToString()));
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
