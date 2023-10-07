using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.ListEv
{
    class TytGdskGetListEvBehaviorFactory
    {
        internal static ITytGdskGetListEv MakeITytGdskGetListEv(CommonParam param, object data)
        {
            ITytGdskGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytGdskFilterQuery))
                {
                    result = new TytGdskGetListEvBehaviorByFilterQuery(param, (TytGdskFilterQuery)data);
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
