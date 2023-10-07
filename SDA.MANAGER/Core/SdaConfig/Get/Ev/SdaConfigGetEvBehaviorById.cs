using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfig.Get.Ev
{
    class SdaConfigGetEvBehaviorById : BeanObjectBase, ISdaConfigGetEv
    {
        long id;

        internal SdaConfigGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_CONFIG ISdaConfigGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigDAO.GetById(id, new SdaConfigFilterQuery().Query());
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
