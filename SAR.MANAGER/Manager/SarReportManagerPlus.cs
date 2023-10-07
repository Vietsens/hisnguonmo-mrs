using SAR.MANAGER.Core.SarReport;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Manager
{
    public partial class SarReportManager : ManagerBase
    {
        public object UpdateStt(object data)
        {
            object result = null;
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SarReportBO bo = new SarReportBO();
                if (bo.UpdateStt(data))
                {
                    result = data;
                }
                CopyCommonParamInfo(bo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
