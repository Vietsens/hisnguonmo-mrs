using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLicense.Get.Ev
{
    class SdaLicenseGetEvBehaviorFactory
    {
        internal static ISdaLicenseGetEv MakeISdaLicenseGetEv(CommonParam param, object data)
        {
            ISdaLicenseGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new SdaLicenseGetEvBehaviorById(param, long.Parse(data.ToString()));
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
