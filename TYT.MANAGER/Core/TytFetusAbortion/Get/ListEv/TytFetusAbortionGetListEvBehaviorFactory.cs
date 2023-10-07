using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.ListEv
{
    class TytFetusAbortionGetListEvBehaviorFactory
    {
        internal static ITytFetusAbortionGetListEv MakeITytFetusAbortionGetListEv(CommonParam param, object data)
        {
            ITytFetusAbortionGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytFetusAbortionFilterQuery))
                {
                    result = new TytFetusAbortionGetListEvBehaviorByFilterQuery(param, (TytFetusAbortionFilterQuery)data);
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
