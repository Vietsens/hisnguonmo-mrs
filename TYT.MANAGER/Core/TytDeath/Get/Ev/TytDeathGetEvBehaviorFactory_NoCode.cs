using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytDeath.Get.Ev
{
    class TytDeathGetEvBehaviorFactory
    {
        internal static ITytDeathGetEv MakeITytDeathGetEv(CommonParam param, object data)
        {
            ITytDeathGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new TytDeathGetEvBehaviorById(param, long.Parse(data.ToString()));
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
