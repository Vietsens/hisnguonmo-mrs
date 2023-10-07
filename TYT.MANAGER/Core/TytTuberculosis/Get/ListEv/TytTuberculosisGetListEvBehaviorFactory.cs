using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.ListEv
{
    class TytTuberculosisGetListEvBehaviorFactory
    {
        internal static ITytTuberculosisGetListEv MakeITytTuberculosisGetListEv(CommonParam param, object data)
        {
            ITytTuberculosisGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytTuberculosisFilterQuery))
                {
                    result = new TytTuberculosisGetListEvBehaviorByFilterQuery(param, (TytTuberculosisFilterQuery)data);
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
