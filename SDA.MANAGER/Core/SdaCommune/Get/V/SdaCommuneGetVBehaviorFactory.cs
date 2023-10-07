using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.V
{
    class SdaCommuneGetVBehaviorFactory
    {
        internal static ISdaCommuneGetV MakeISdaCommuneGetV(CommonParam param, object data)
        {
            ISdaCommuneGetV result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SdaCommuneGetVBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SdaCommuneGetVBehaviorById(param, long.Parse(data.ToString()));
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
