using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaCommune.Get.ListEv
{
    class SdaCommuneGetListEvBehaviorFactory
    {
        internal static ISdaCommuneGetListEv MakeISdaCommuneGetListEv(CommonParam param, object data)
        {
            ISdaCommuneGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaCommuneFilterQuery))
                {
                    result = new SdaCommuneGetListEvBehaviorByFilterQuery(param, (SdaCommuneFilterQuery)data);
                }
                else if (data.GetType() == typeof(List<long>))
                {
                    result = new SdaCommuneGetListEvByDistrictBehaviorByFilterQuery(param, (List<long>)data);
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
