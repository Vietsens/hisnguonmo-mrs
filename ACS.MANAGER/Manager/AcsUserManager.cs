using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser;
using Inventec.Core;
using System;

namespace ACS.MANAGER.Manager
{
    public partial class AcsUserManager : Inventec.Backend.MANAGER.ManagerBase
    {
        public AcsUserManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                AcsUserBO bo = new AcsUserBO();
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
