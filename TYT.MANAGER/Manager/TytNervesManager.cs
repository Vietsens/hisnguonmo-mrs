using TYT.MANAGER.Core.TytNerves;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Manager
{
    public partial class TytNervesManager : ManagerBase
    {
        public TytNervesManager(CommonParam param)
            : base(param)
        {

        }

        public T Get<T>(object data)
        {
            T result = default(T);
            try
            {
                if (!IsNotNull(data)) throw new ArgumentNullException("data");
                TytNervesBO bo = new TytNervesBO();
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
