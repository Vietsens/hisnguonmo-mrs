using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.ListEv
{
    class SdaNationalGetListEvBehaviorFactory
    {
        internal static ISdaNationalGetListEv MakeISdaNationalGetListEv(CommonParam param, object data)
        {
            ISdaNationalGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaNationalFilterQuery))
                {
                    result = new SdaNationalGetListEvBehaviorByFilterQuery(param, (SdaNationalFilterQuery)data);
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
