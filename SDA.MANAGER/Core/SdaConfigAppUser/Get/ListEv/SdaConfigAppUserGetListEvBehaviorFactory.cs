using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.ListEv
{
    class SdaConfigAppUserGetListEvBehaviorFactory
    {
        internal static ISdaConfigAppUserGetListEv MakeISdaConfigAppUserGetListEv(CommonParam param, object data)
        {
            ISdaConfigAppUserGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaConfigAppUserFilterQuery))
                {
                    result = new SdaConfigAppUserGetListEvBehaviorByFilterQuery(param, (SdaConfigAppUserFilterQuery)data);
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
