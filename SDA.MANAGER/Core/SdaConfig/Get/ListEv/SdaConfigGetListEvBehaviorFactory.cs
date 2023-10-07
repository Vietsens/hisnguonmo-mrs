using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfig.Get.ListEv
{
    class SdaConfigGetListEvBehaviorFactory
    {
        internal static ISdaConfigGetListEv MakeISdaConfigGetListEv(CommonParam param, object data)
        {
            ISdaConfigGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaConfigFilterQuery))
                {
                    result = new SdaConfigGetListEvBehaviorByFilterQuery(param, (SdaConfigFilterQuery)data);
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
