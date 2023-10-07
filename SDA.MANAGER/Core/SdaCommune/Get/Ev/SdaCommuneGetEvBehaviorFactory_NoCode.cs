using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.Ev
{
    class SdaCommuneGetEvBehaviorFactory
    {
        internal static ISdaCommuneGetEv MakeISdaCommuneGetEv(CommonParam param, object data)
        {
            ISdaCommuneGetEv result = null;
            try
            {
                if (data.GetType() == typeof(long))
                {
                    result = new SdaCommuneGetEvBehaviorById(param, long.Parse(data.ToString()));
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
