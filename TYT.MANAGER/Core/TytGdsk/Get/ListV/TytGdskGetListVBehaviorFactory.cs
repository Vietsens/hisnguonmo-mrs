using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.ListV
{
    class TytGdskGetListVBehaviorFactory
    {
        internal static ITytGdskGetListV MakeITytGdskGetListV(CommonParam param, object data)
        {
            ITytGdskGetListV result = null;
            try
            {
                if (data.GetType() == typeof(TytGdskViewFilterQuery))
                {
                    result = new TytGdskGetListVBehaviorByViewFilterQuery(param, (TytGdskViewFilterQuery)data);
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
