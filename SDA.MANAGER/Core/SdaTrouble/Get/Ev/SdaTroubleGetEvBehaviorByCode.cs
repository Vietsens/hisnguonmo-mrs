using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTrouble.Get.Ev
{
    class SdaTroubleGetEvBehaviorByCode : BeanObjectBase, ISdaTroubleGetEv
    {
        string code;

        internal SdaTroubleGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_TROUBLE ISdaTroubleGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaTroubleDAO.GetByCode(code, new SdaTroubleFilterQuery().Query());
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
