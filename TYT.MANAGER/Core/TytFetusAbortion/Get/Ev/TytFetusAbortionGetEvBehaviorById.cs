using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.Ev
{
    class TytFetusAbortionGetEvBehaviorById : BeanObjectBase, ITytFetusAbortionGetEv
    {
        long id;

        internal TytFetusAbortionGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_FETUS_ABORTION ITytFetusAbortionGetEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusAbortionDAO.GetById(id, new TytFetusAbortionFilterQuery().Query());
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
