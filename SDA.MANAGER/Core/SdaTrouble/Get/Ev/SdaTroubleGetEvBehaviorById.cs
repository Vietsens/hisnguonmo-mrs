using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTrouble.Get.Ev
{
    class SdaTroubleGetEvBehaviorById : BeanObjectBase, ISdaTroubleGetEv
    {
        long id;

        internal SdaTroubleGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_TROUBLE ISdaTroubleGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaTroubleDAO.GetById(id, new SdaTroubleFilterQuery().Query());
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
