using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.ListEv
{
    class SdaGroupGetListEvBehaviorFactory
    {
        internal static ISdaGroupGetListEv MakeISdaGroupGetListEv(CommonParam param, object data)
        {
            ISdaGroupGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaGroupFilterQuery))
                {
                    result = new SdaGroupGetListEvBehaviorByFilterQuery(param, (SdaGroupFilterQuery)data);
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
