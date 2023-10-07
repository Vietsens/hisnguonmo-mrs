using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaNational.Get.Ev
{
    class SdaNationalGetEvBehaviorById : BeanObjectBase, ISdaNationalGetEv
    {
        long id;

        internal SdaNationalGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_NATIONAL ISdaNationalGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaNationalDAO.GetById(id, new SdaNationalFilterQuery().Query());
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
