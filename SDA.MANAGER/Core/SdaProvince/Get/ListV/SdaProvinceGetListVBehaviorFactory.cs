using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaProvince.Get.ListV
{
    class SdaProvinceGetListVBehaviorFactory
    {
        internal static ISdaProvinceGetListV MakeISdaProvinceGetListV(CommonParam param, object data)
        {
            ISdaProvinceGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SdaProvinceViewFilterQuery))
                {
                    result = new SdaProvinceGetListVBehaviorByViewFilterQuery(param, (SdaProvinceViewFilterQuery)data);
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
