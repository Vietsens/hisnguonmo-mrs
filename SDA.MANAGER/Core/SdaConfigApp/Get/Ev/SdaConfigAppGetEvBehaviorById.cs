using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaConfigApp.Get.Ev
{
    class SdaConfigAppGetEvBehaviorById : BeanObjectBase, ISdaConfigAppGetEv
    {
        long id;

        internal SdaConfigAppGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_CONFIG_APP ISdaConfigAppGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaConfigAppDAO.GetById(id, new SdaConfigAppFilterQuery().Query());
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
