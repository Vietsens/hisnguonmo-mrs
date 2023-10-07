using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaGroupType.Get.ListEv
{
    class SdaGroupTypeGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaGroupTypeGetListEv
    {
        SdaGroupTypeFilterQuery filterQuery;

        internal SdaGroupTypeGetListEvBehaviorByFilterQuery(CommonParam param, SdaGroupTypeFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_GROUP_TYPE> ISdaGroupTypeGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupTypeDAO.Get(filterQuery.Query(), param);
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
