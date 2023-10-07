using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace ACS.MANAGER.Core.AcsUser.Get.ListEv
{
    class AcsUserGetListEvBehaviorByFilterQuery : BeanObjectBase, IAcsUserGetListEv
    {
        AcsUserFilterQuery filterQuery;

        internal AcsUserGetListEvBehaviorByFilterQuery(CommonParam param, AcsUserFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<ACS_USER> IAcsUserGetListEv.Run()
        {
            try
            {
                return DAOWorker.AcsUserDAO.Get(filterQuery.Query(), param);
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
