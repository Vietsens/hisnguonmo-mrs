using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.Ev
{
    class TytFetusAbortionGetEvBehaviorFactory
    {
        internal static ITytFetusAbortionGetEv MakeITytFetusAbortionGetEv(CommonParam param, object data)
        {
            ITytFetusAbortionGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new TytFetusAbortionGetEvBehaviorById(param, long.Parse(data.ToString()));
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
