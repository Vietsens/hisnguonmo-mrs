using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsUser.Get.Ev
{
    class AcsUserGetEvBehaviorFactory
    {
        internal static IAcsUserGetEv MakeIAcsUserGetEv(CommonParam param, object data)
        {
            IAcsUserGetEv result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new AcsUserGetEvBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new AcsUserGetEvBehaviorById(param, long.Parse(data.ToString()));
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                //
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
