using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.Ev
{
    class SdaConfigAppUserGetEvBehaviorById : BeanObjectBase, ISdaConfigAppUserGetEv
    {
        long id;

        internal SdaConfigAppUserGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_CONFIG_APP_USER ISdaConfigAppUserGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppUserDAO.GetById(id, new SdaConfigAppUserFilterQuery().Query());
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
