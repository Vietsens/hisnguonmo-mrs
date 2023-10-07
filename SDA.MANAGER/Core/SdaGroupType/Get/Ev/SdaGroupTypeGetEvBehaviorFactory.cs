using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroupType.Get.Ev
{
    class SdaGroupTypeGetEvBehaviorFactory
    {
        internal static ISdaGroupTypeGetEv MakeISdaGroupTypeGetEv(CommonParam param, object data)
        {
            ISdaGroupTypeGetEv result = null;
            try
            {
                if (data.GetType() == typeof(string))
                {
                    result = new SdaGroupTypeGetEvBehaviorByCode(param, data.ToString());
                }
                else if (data.GetType() == typeof(long))
                {
                    result = new SdaGroupTypeGetEvBehaviorById(param, long.Parse(data.ToString()));
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
