using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.ListEv
{
    class TytUninfectIcdGroupGetListEvBehaviorFactory
    {
        internal static ITytUninfectIcdGroupGetListEv MakeITytUninfectIcdGroupGetListEv(CommonParam param, object data)
        {
            ITytUninfectIcdGroupGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytUninfectIcdGroupFilterQuery))
                {
                    result = new TytUninfectIcdGroupGetListEvBehaviorByFilterQuery(param, (TytUninfectIcdGroupFilterQuery)data);
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
