using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytGdsk.Get.ListEv
{
    class TytGdskGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytGdskGetListEv
    {
        TytGdskFilterQuery filterQuery;

        internal TytGdskGetListEvBehaviorByFilterQuery(CommonParam param, TytGdskFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_GDSK> ITytGdskGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytGdskDAO.Get(filterQuery.Query(), param);
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
