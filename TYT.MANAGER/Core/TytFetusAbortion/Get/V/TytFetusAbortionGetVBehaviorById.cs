using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.V
{
    class TytFetusAbortionGetVBehaviorById : BeanObjectBase, ITytFetusAbortionGetV
    {
        long id;

        internal TytFetusAbortionGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_FETUS_ABORTION ITytFetusAbortionGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusAbortionDAO.GetViewById(id, new TytFetusAbortionViewFilterQuery().Query());
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
