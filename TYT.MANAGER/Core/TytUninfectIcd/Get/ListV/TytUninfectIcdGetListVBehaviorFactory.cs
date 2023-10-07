using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.ListV
{
    class TytUninfectIcdGetListVBehaviorFactory
    {
        internal static ITytUninfectIcdGetListV MakeITytUninfectIcdGetListV(CommonParam param, object data)
        {
            ITytUninfectIcdGetListV result = null;
            try
            {
                if (data.GetType() == typeof(TytUninfectIcdViewFilterQuery))
                {
                    result = new TytUninfectIcdGetListVBehaviorByViewFilterQuery(param, (TytUninfectIcdViewFilterQuery)data);
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
