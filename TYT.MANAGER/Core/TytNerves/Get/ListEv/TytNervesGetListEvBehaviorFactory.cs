using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.ListEv
{
    class TytNervesGetListEvBehaviorFactory
    {
        internal static ITytNervesGetListEv MakeITytNervesGetListEv(CommonParam param, object data)
        {
            ITytNervesGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytNervesFilterQuery))
                {
                    result = new TytNervesGetListEvBehaviorByFilterQuery(param, (TytNervesFilterQuery)data);
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
