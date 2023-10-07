using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.V
{
    class SdaCommuneGetVBehaviorByCode : BeanObjectBase, ISdaCommuneGetV
    {
        string code;

        internal SdaCommuneGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SDA_COMMUNE ISdaCommuneGetV.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.GetViewByCode(code, new SdaCommuneViewFilterQuery().Query());
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
