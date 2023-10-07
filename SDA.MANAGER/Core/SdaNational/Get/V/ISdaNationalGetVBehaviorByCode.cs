using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.V
{
    class SdaNationalGetVBehaviorByCode : BeanObjectBase, ISdaNationalGetV
    {
        string code;

        internal SdaNationalGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SDA_NATIONAL ISdaNationalGetV.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.GetViewByCode(code, new SdaNationalViewFilterQuery().Query());
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
