using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaDistrict.Get.ListV
{
    class SdaDistrictGetListVBehaviorFactory
    {
        internal static ISdaDistrictGetListV MakeISdaDistrictGetListV(CommonParam param, object data)
        {
            ISdaDistrictGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SdaDistrictViewFilterQuery))
                {
                    result = new SdaDistrictGetListVBehaviorByViewFilterQuery(param, (SdaDistrictViewFilterQuery)data);
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
