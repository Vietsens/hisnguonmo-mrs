using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaProvince.Get.ListEv
{
    class SdaProvinceGetListEvBehaviorFactory
    {
        internal static ISdaProvinceGetListEv MakeISdaProvinceGetListEv(CommonParam param, object data)
        {
            ISdaProvinceGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaProvinceFilterQuery))
                {
                    result = new SdaProvinceGetListEvBehaviorByFilterQuery(param, (SdaProvinceFilterQuery)data);
                }
                else if (data.GetType() == typeof(List<long>))
                {
                    result = new SdaProvinceGetListEvBehaviorByNationalIds(param, (List<long>)data);
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
