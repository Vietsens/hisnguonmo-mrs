using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.Ev
{
    class TytNervesGetEvBehaviorFactory
    {
        internal static ITytNervesGetEv MakeITytNervesGetEv(CommonParam param, object data)
        {
            ITytNervesGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new TytNervesGetEvBehaviorById(param, long.Parse(data.ToString()));
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
