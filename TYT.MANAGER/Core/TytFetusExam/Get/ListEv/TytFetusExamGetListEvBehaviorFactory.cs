using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusExam.Get.ListEv
{
    class TytFetusExamGetListEvBehaviorFactory
    {
        internal static ITytFetusExamGetListEv MakeITytFetusExamGetListEv(CommonParam param, object data)
        {
            ITytFetusExamGetListEv result = null;
            try
            {
                if (data.GetType() == typeof(TytFetusExamFilterQuery))
                {
                    result = new TytFetusExamGetListEvBehaviorByFilterQuery(param, (TytFetusExamFilterQuery)data);
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
