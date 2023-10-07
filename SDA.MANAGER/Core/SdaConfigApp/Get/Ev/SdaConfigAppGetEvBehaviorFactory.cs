using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigApp.Get.Ev
{
    class SdaConfigAppGetEvBehaviorFactory
    {
        internal static ISdaConfigAppGetEv MakeISdaConfigAppGetEv(CommonParam param, object data)
        {
            ISdaConfigAppGetEv result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SdaConfigAppGetEvBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SdaConfigAppGetEvBehaviorById(param, long.Parse(data.ToString()));
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
