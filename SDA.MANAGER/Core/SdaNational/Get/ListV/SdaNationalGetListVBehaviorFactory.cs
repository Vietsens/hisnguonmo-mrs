using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.ListV
{
    class SdaNationalGetListVBehaviorFactory
    {
        internal static ISdaNationalGetListV MakeISdaNationalGetListV(CommonParam param, object data)
        {
            ISdaNationalGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SdaNationalViewFilterQuery))
                {
                    result = new SdaNationalGetListVBehaviorByViewFilterQuery(param, (SdaNationalViewFilterQuery)data);
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
