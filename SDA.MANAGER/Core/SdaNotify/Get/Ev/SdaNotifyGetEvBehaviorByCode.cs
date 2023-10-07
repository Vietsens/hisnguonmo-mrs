using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNotify.Get.Ev
{
    class SdaNotifyGetEvBehaviorByCode : BeanObjectBase, ISdaNotifyGetEv
    {
        string code;

        internal SdaNotifyGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_NOTIFY ISdaNotifyGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaNotifyDAO.GetByCode(code, new SdaNotifyFilterQuery().Query());
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
