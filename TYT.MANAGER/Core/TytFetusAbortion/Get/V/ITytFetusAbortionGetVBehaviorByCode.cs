using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.V
{
    class TytFetusAbortionGetVBehaviorByCode : BeanObjectBase, ITytFetusAbortionGetV
    {
        string code;

        internal TytFetusAbortionGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_FETUS_ABORTION ITytFetusAbortionGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusAbortionDAO.GetViewByCode(code, new TytFetusAbortionViewFilterQuery().Query());
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
