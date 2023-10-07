using Inventec.Core;
using System;

namespace ACS.MANAGER.Core.AcsUser.Get.ListEv
{
    class AcsUserGetListEvBehaviorFactory
    {
        internal static IAcsUserGetListEv MakeIAcsUserGetListEv(CommonParam param, object data)
        {
            IAcsUserGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(AcsUserFilterQuery))
                {
                    result = new AcsUserGetListEvBehaviorByFilterQuery(param, (AcsUserFilterQuery)data);
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
