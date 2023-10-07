using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNotify.Get.Ev
{
    class SdaNotifyGetEvBehaviorById : BeanObjectBase, ISdaNotifyGetEv
    {
        long id;

        internal SdaNotifyGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_NOTIFY ISdaNotifyGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaNotifyDAO.GetById(id, new SdaNotifyFilterQuery().Query());
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
