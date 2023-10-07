using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaReligion.Get.ListEv
{
    class SdaReligionGetListEvBehaviorFactory
    {
        internal static ISdaReligionGetListEv MakeISdaReligionGetListEv(CommonParam param, object data)
        {
            ISdaReligionGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaReligionFilterQuery))
                {
                    result = new SdaReligionGetListEvBehaviorByFilterQuery(param, (SdaReligionFilterQuery)data);
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
