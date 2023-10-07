using SDA.MANAGER.Core.SdaReligion;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Manager
{
    public partial class SdaReligionManager : ManagerBase
    {
        public SdaReligionManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                SdaReligionBO bo = new SdaReligionBO();
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
