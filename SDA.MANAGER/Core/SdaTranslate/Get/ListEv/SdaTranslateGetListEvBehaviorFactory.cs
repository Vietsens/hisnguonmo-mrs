using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTranslate.Get.ListEv
{
    class SdaTranslateGetListEvBehaviorFactory
    {
        internal static ISdaTranslateGetListEv MakeISdaTranslateGetListEv(CommonParam param, object data)
        {
            ISdaTranslateGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaTranslateFilterQuery))
                {
                    result = new SdaTranslateGetListEvBehaviorByFilterQuery(param, (SdaTranslateFilterQuery)data);
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
