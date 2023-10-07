using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytKhh.Get.ListEv
{
    class TytKhhGetListEvBehaviorFactory
    {
        internal static ITytKhhGetListEv MakeITytKhhGetListEv(CommonParam param, object data)
        {
            ITytKhhGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytKhhFilterQuery))
                {
                    result = new TytKhhGetListEvBehaviorByFilterQuery(param, (TytKhhFilterQuery)data);
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
