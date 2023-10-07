using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.ListEv
{
    class TytUninfectIcdGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytUninfectIcdGetListEv
    {
        TytUninfectIcdFilterQuery filterQuery;

        internal TytUninfectIcdGetListEvBehaviorByFilterQuery(CommonParam param, TytUninfectIcdFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_UNINFECT_ICD> ITytUninfectIcdGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.Get(filterQuery.Query(), param);
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
