using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytMalaria.Get.ListEv
{
    class TytMalariaGetListEvBehaviorFactory
    {
        internal static ITytMalariaGetListEv MakeITytMalariaGetListEv(CommonParam param, object data)
        {
            ITytMalariaGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytMalariaFilterQuery))
                {
                    result = new TytMalariaGetListEvBehaviorByFilterQuery(param, (TytMalariaFilterQuery)data);
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
