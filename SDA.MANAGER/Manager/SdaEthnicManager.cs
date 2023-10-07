using SDA.MANAGER.Core.SdaEthnic;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaEthnicManager : ManagerBase
    {
        public SdaEthnicManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaEthnicBO bo = new SdaEthnicBO();
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
