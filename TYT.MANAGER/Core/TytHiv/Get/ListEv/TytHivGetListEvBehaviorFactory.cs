using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytHiv.Get.ListEv
{
    class TytHivGetListEvBehaviorFactory
    {
        internal static ITytHivGetListEv MakeITytHivGetListEv(CommonParam param, object data)
        {
            ITytHivGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytHivFilterQuery))
                {
                    result = new TytHivGetListEvBehaviorByFilterQuery(param, (TytHivFilterQuery)data);
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
