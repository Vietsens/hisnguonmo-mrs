using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEthnic.Get.ListEv
{
    class SdaEthnicGetListEvBehaviorFactory
    {
        internal static ISdaEthnicGetListEv MakeISdaEthnicGetListEv(CommonParam param, object data)
        {
            ISdaEthnicGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaEthnicFilterQuery))
                {
                    result = new SdaEthnicGetListEvBehaviorByFilterQuery(param, (SdaEthnicFilterQuery)data);
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
