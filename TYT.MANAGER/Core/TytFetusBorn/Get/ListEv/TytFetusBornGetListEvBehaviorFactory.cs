using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.ListEv
{
    class TytFetusBornGetListEvBehaviorFactory
    {
        internal static ITytFetusBornGetListEv MakeITytFetusBornGetListEv(CommonParam param, object data)
        {
            ITytFetusBornGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytFetusBornFilterQuery))
                {
                    result = new TytFetusBornGetListEvBehaviorByFilterQuery(param, (TytFetusBornFilterQuery)data);
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
