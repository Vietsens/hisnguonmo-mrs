using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.ListEv
{
    class TytUninfectIcdGroupGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytUninfectIcdGroupGetListEv
    {
        TytUninfectIcdGroupFilterQuery filterQuery;

        internal TytUninfectIcdGroupGetListEvBehaviorByFilterQuery(CommonParam param, TytUninfectIcdGroupFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_UNINFECT_ICD_GROUP> ITytUninfectIcdGroupGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.Get(filterQuery.Query(), param);
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
