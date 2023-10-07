using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaReligion.Get.ListEv
{
    class SdaReligionGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaReligionGetListEv
    {
        SdaReligionFilterQuery filterQuery;

        internal SdaReligionGetListEvBehaviorByFilterQuery(CommonParam param, SdaReligionFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_RELIGION> ISdaReligionGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaReligionDAO.Get(filterQuery.Query(), param);
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
