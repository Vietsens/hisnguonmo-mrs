using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsRole.Get.ListEv
{
    class AcsRoleGetListEvBehaviorFactory
    {
        internal static IAcsRoleGetListEv MakeIAcsRoleGetListEv(CommonParam param, object data)
        {
            IAcsRoleGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(AcsRoleFilterQuery))
                {
                    result = new AcsRoleGetListEvBehaviorByFilterQuery(param, (AcsRoleFilterQuery)data);
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
