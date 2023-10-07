using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.Ev
{
    class TytGdskGetEvBehaviorFactory
    {
        internal static ITytGdskGetEv MakeITytGdskGetEv(CommonParam param, object data)
        {
            ITytGdskGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new TytGdskGetEvBehaviorById(param, long.Parse(data.ToString()));
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
