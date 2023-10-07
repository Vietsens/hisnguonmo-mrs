using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigApp.Get.Ev
{
    class SdaConfigAppGetEvBehaviorByCode : BeanObjectBase, ISdaConfigAppGetEv
    {
        string code;

        internal SdaConfigAppGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_CONFIG_APP ISdaConfigAppGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppDAO.GetByCode(code, new SdaConfigAppFilterQuery().Query());
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
