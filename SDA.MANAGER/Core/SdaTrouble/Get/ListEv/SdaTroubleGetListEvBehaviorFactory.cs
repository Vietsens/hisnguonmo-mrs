using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTrouble.Get.ListEv
{
    class SdaTroubleGetListEvBehaviorFactory
    {
        internal static ISdaTroubleGetListEv MakeISdaTroubleGetListEv(CommonParam param, object data)
        {
            ISdaTroubleGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaTroubleFilterQuery))
                {
                    result = new SdaTroubleGetListEvBehaviorByFilterQuery(param, (SdaTroubleFilterQuery)data);
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
