using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.ListV
{
    class SdaCommuneGetListVBehaviorFactory
    {
        internal static ISdaCommuneGetListV MakeISdaCommuneGetListV(CommonParam param, object data)
        {
            ISdaCommuneGetListV result = null;
            try
            {
                if (data.GetType() == typeof(SdaCommuneViewFilterQuery))
                {
                    result = new SdaCommuneGetListVBehaviorByViewFilterQuery(param, (SdaCommuneViewFilterQuery)data);
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
