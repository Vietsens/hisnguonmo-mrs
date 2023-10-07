using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLicense.Get.GetLast
{
    class SdaLicenseGetLastBehaviorFactory
    {
        internal static ISdaLicenseGetLast MakeISdaLicenseGetLast(CommonParam param, object data)
        {
            ISdaLicenseGetLast result = null;
            try
            {
                if (data.GetType() == typeof(SdaLicenseFilterQuery))
                {
                    result = new SdaLicenseGetLastBehavior(param, (SdaLicenseFilterQuery)(data));
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
