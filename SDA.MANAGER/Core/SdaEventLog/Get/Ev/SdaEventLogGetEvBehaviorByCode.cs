using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEventLog.Get.Ev
{
    class SdaEventLogGetEvBehaviorByCode : BeanObjectBase, ISdaEventLogGetEv
    {
        string code;

        internal SdaEventLogGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_EVENT_LOG ISdaEventLogGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaEventLogDAO.GetByCode(code, new SdaEventLogFilterQuery().Query());
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
