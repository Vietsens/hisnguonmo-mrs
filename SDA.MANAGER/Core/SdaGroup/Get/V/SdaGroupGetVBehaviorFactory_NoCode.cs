using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.V
{
    class SdaGroupGetVBehaviorFactory
    {
        internal static ISdaGroupGetV MakeISdaGroupGetV(CommonParam param, object data)
        {
            ISdaGroupGetV result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new SdaGroupGetVBehaviorById(param, long.Parse(data.ToString()));
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
