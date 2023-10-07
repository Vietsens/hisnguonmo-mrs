using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.V
{
    class SdaConfigAppUserGetVBehaviorById : BeanObjectBase, ISdaConfigAppUserGetV
    {
        long id;

        internal SdaConfigAppUserGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_CONFIG_APP_USER ISdaConfigAppUserGetV.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppUserDAO.GetViewById(id, new SdaConfigAppUserViewFilterQuery().Query());
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
