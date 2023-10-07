using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLanguage.Get.ListEv
{
    class SdaLanguageGetListEvBehaviorFactory
    {
        internal static ISdaLanguageGetListEv MakeISdaLanguageGetListEv(CommonParam param, object data)
        {
            ISdaLanguageGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(SdaLanguageFilterQuery))
                {
                    result = new SdaLanguageGetListEvBehaviorByFilterQuery(param, (SdaLanguageFilterQuery)data);
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
