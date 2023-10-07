using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEventLog.Get.Ev
{
    class SdaEventLogGetEvBehaviorById : BeanObjectBase, ISdaEventLogGetEv
    {
        long id;

        internal SdaEventLogGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_EVENT_LOG ISdaEventLogGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaEventLogDAO.GetById(id, new SdaEventLogFilterQuery().Query());
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
