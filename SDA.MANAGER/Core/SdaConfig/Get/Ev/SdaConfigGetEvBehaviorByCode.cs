using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfig.Get.Ev
{
    class SdaConfigGetEvBehaviorByCode : BeanObjectBase, ISdaConfigGetEv
    {
        string code;

        internal SdaConfigGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_CONFIG ISdaConfigGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigDAO.GetByCode(code, new SdaConfigFilterQuery().Query());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
