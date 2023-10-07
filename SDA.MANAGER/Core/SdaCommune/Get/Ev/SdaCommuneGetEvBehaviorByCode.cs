using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaCommune.Get.Ev
{
    class SdaCommuneGetEvBehaviorByCode : BeanObjectBase, ISdaCommuneGetEv
    {
        string code;

        internal SdaCommuneGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_COMMUNE ISdaCommuneGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaCommuneDAO.GetByCode(code, new SdaCommuneFilterQuery().Query());
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
