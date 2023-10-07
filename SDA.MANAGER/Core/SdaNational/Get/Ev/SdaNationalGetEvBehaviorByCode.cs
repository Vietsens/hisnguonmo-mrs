using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.Ev
{
    class SdaNationalGetEvBehaviorByCode : BeanObjectBase, ISdaNationalGetEv
    {
        string code;

        internal SdaNationalGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_NATIONAL ISdaNationalGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.GetByCode(code, new SdaNationalFilterQuery().Query());
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
