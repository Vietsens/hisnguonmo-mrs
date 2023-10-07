using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.Ev
{
    class SdaCommuneGetEvBehaviorById : BeanObjectBase, ISdaCommuneGetEv
    {
        long id;

        internal SdaCommuneGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_COMMUNE ISdaCommuneGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.GetById(id, new SdaCommuneFilterQuery().Query());
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
