using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroupType.Get.ListEv
{
    class SdaGroupTypeGetListEvBehaviorFactory
    {
        internal static ISdaGroupTypeGetListEv MakeISdaGroupTypeGetListEv(CommonParam param, object data)
        {
            ISdaGroupTypeGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaGroupTypeFilterQuery))
                {
                    result = new SdaGroupTypeGetListEvBehaviorByFilterQuery(param, (SdaGroupTypeFilterQuery)data);
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
