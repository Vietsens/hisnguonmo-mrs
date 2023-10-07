using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigAppUser.Get.Ev
{
    class SdaConfigAppUserGetEvBehaviorByCode : BeanObjectBase, ISdaConfigAppUserGetEv
    {
        string code;

        internal SdaConfigAppUserGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_CONFIG_APP_USER ISdaConfigAppUserGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppUserDAO.GetByCode(code, new SdaConfigAppUserFilterQuery().Query());
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
