using SDA.MANAGER.Core.SdaTranslate;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaTranslateManager : ManagerBase
    {
        public SdaTranslateManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaTranslateBO bo = new SdaTranslateBO();
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
