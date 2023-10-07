using SDA.MANAGER.Core.SdaConfig;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaConfigManager : ManagerBase
    {
        public SdaConfigManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaConfigBO bo = new SdaConfigBO();
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
