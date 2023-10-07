using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.ListEv
{
    class TytUninfectIcdGetListEvBehaviorFactory
    {
        internal static ITytUninfectIcdGetListEv MakeITytUninfectIcdGetListEv(CommonParam param, object data)
        {
            ITytUninfectIcdGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytUninfectIcdFilterQuery))
                {
                    result = new TytUninfectIcdGetListEvBehaviorByFilterQuery(param, (TytUninfectIcdFilterQuery)data);
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
