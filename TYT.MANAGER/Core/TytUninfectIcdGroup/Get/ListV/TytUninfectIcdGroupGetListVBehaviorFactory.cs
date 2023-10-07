using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.ListV
{
    class TytUninfectIcdGroupGetListVBehaviorFactory
    {
        internal static ITytUninfectIcdGroupGetListV MakeITytUninfectIcdGroupGetListV(CommonParam param, object data)
        {
            ITytUninfectIcdGroupGetListV result = null;
            try
            {
                if (data.GetType() == typeof(TytUninfectIcdGroupViewFilterQuery))
                {
                    result = new TytUninfectIcdGroupGetListVBehaviorByViewFilterQuery(param, (TytUninfectIcdGroupViewFilterQuery)data);
                }
                if (result == null) throw new NullReferenceException();
            }
            catch (NullReferenceException ex)
            {
                MANAGER.Base.BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__FactoryKhoiTaoDoiTuongThatBai);
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
