using SDA.MANAGER.Core.SdaConfigApp;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaConfigAppManager : ManagerBase
    {
        public SdaConfigAppManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaConfigAppBO bo = new SdaConfigAppBO();
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
