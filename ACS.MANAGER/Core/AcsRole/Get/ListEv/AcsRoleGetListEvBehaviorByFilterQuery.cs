using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACS.MANAGER.Core.AcsRole.Get.ListEv
{
    class AcsRoleGetListEvBehaviorByFilterQuery : BeanObjectBase, IAcsRoleGetListEv
    {
        AcsRoleFilterQuery filterQuery;

        internal AcsRoleGetListEvBehaviorByFilterQuery(CommonParam param, AcsRoleFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<ACS_ROLE> IAcsRoleGetListEv.Run()
        {
            try
            {
                return DAOWorker.AcsRoleDAO.Get(filterQuery.Query(), param);
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
