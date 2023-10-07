using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.ListV
{
    class TytNervesGetListVBehaviorFactory
    {
        internal static ITytNervesGetListV MakeITytNervesGetListV(CommonParam param, object data)
        {
            ITytNervesGetListV result = null;
            try
            {
                if (data.GetType() == typeof(TytNervesViewFilterQuery))
                {
                    result = new TytNervesGetListVBehaviorByViewFilterQuery(param, (TytNervesViewFilterQuery)data);
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
