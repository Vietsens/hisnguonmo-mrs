using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytDeath.Get.ListEv
{
    class TytDeathGetListEvBehaviorFactory
    {
        internal static ITytDeathGetListEv MakeITytDeathGetListEv(CommonParam param, object data)
        {
            ITytDeathGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytDeathFilterQuery))
                {
                    result = new TytDeathGetListEvBehaviorByFilterQuery(param, (TytDeathFilterQuery)data);
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
