using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.V
{
    class SdaNationalGetVBehaviorById : BeanObjectBase, ISdaNationalGetV
    {
        long id;

        internal SdaNationalGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_NATIONAL ISdaNationalGetV.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.GetViewById(id, new SdaNationalViewFilterQuery().Query());
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
