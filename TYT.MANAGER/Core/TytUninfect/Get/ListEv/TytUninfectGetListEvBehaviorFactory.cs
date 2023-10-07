using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfect.Get.ListEv
{
    class TytUninfectGetListEvBehaviorFactory
    {
        internal static ITytUninfectGetListEv MakeITytUninfectGetListEv(CommonParam param, object data)
        {
            ITytUninfectGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytUninfectFilterQuery))
                {
                    result = new TytUninfectGetListEvBehaviorByFilterQuery(param, (TytUninfectFilterQuery)data);
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
