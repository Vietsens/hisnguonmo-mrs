using TYT.MANAGER.Core.TytDeath;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Manager
{
    public partial class TytDeathManager : ManagerBase
    {
        public TytDeathManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                TytDeathBO bo = new TytDeathBO();
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
