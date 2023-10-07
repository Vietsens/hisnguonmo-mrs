using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.V
{
    class SdaCommuneGetVBehaviorById : BeanObjectBase, ISdaCommuneGetV
    {
        long id;

        internal SdaCommuneGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_COMMUNE ISdaCommuneGetV.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.GetViewById(id, new SdaCommuneViewFilterQuery().Query());
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
