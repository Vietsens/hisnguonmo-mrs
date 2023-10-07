using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.ListEv
{
    class TytFetusAbortionGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytFetusAbortionGetListEv
    {
        TytFetusAbortionFilterQuery filterQuery;

        internal TytFetusAbortionGetListEvBehaviorByFilterQuery(CommonParam param, TytFetusAbortionFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_FETUS_ABORTION> ITytFetusAbortionGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusAbortionDAO.Get(filterQuery.Query(), param);
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
