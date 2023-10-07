using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsRole.Get.Ev
{
    class AcsRoleGetEvBehaviorFactory
    {
        internal static IAcsRoleGetEv MakeIAcsRoleGetEv(CommonParam param, object data)
        {
            IAcsRoleGetEv result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new AcsRoleGetEvBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new AcsRoleGetEvBehaviorById(param, long.Parse(data.ToString()));
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
