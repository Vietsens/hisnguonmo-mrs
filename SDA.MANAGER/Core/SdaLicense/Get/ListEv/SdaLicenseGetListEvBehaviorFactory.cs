using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLicense.Get.ListEv
{
    class SdaLicenseGetListEvBehaviorFactory
    {
        internal static ISdaLicenseGetListEv MakeISdaLicenseGetListEv(CommonParam param, object data)
        {
            ISdaLicenseGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaLicenseFilterQuery))
                {
                    result = new SdaLicenseGetListEvBehaviorByFilterQuery(param, (SdaLicenseFilterQuery)data);
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
