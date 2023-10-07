using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytHiv.Get.Ev
{
    class TytHivGetEvBehaviorFactory
    {
        internal static ITytHivGetEv MakeITytHivGetEv(CommonParam param, object data)
        {
            ITytHivGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new TytHivGetEvBehaviorById(param, long.Parse(data.ToString()));
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
