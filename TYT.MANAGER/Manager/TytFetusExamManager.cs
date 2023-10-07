using TYT.MANAGER.Core.TytFetusExam;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Manager
{
    public partial class TytFetusExamManager : ManagerBase
    {
        public TytFetusExamManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                TytFetusExamBO bo = new TytFetusExamBO();
                bo.CopyCommonParamInfoGet(param);
                result = bo.Get<T>(data);
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = default(T);
            }
            return result;
        }
    }
}
