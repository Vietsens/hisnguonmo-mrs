using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigApp.Get.ListEv
{
    class SdaConfigAppGetListEvBehaviorFactory
    {
        internal static ISdaConfigAppGetListEv MakeISdaConfigAppGetListEv(CommonParam param, object data)
        {
            ISdaConfigAppGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaConfigAppFilterQuery))
                {
                    result = new SdaConfigAppGetListEvBehaviorByFilterQuery(param, (SdaConfigAppFilterQuery)data);
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
