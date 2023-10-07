using SDA.MANAGER.Core.SdaTrouble;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaTroubleManager : ManagerBase
    {
        public SdaTroubleManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaTroubleBO bo = new SdaTroubleBO();
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
