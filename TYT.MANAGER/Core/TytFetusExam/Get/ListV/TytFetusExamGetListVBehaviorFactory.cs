using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusExam.Get.ListV
{
    class TytFetusExamGetListVBehaviorFactory
    {
        internal static ITytFetusExamGetListV MakeITytFetusExamGetListV(CommonParam param, object data)
        {
            ITytFetusExamGetListV result = null;
            try
            {
                if (data.GetType() == typeof(TytFetusExamViewFilterQuery))
                {
                    result = new TytFetusExamGetListVBehaviorByViewFilterQuery(param, (TytFetusExamViewFilterQuery)data);
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
