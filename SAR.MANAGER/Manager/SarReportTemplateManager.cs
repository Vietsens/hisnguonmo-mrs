using SAR.MANAGER.Core.SarReportTemplate;
using Inventec.Core;
using System;
using AutoMapper;

namespace SAR.MANAGER.Manager
{
    public partial class SarReportTemplateManager : ManagerBase
    {
        public SarReportTemplateManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SarReportTemplateBO bo = new SarReportTemplateBO();
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
