using SAR.MANAGER.Core.SarReportStt;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Manager
{
    public partial class SarReportSttManager : ManagerBase
    {
        public SarReportSttManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SarReportSttBO bo = new SarReportSttBO();
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
