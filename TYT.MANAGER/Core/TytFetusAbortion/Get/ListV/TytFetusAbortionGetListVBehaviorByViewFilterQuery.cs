using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusAbortion.Get.ListV
{
    class TytFetusAbortionGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytFetusAbortionGetListV
    {
        TytFetusAbortionViewFilterQuery filterQuery;

        internal TytFetusAbortionGetListVBehaviorByViewFilterQuery(CommonParam param, TytFetusAbortionViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_FETUS_ABORTION> ITytFetusAbortionGetListV.Run()
        {
            try
            {
                return DAOWorker.TytFetusAbortionDAO.GetView(filterQuery.Query(), param);
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
