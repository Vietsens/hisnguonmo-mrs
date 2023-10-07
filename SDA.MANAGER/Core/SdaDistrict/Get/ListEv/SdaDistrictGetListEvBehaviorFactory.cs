using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict.Get.ListEv
{
    class SdaDistrictGetListEvBehaviorFactory
    {
        internal static ISdaDistrictGetListEv MakeISdaDistrictGetListEv(CommonParam param, object data)
        {
            ISdaDistrictGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaDistrictFilterQuery))
                {
                    result = new SdaDistrictGetListEvBehaviorByFilterQuery(param, (SdaDistrictFilterQuery)data);
                }
                else if (data.GetType() == typeof(List<long>))
                {
                    result = new SdaDistrictGetListEvBehaviorByProvinceIds(param, (List<long>)data);
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
