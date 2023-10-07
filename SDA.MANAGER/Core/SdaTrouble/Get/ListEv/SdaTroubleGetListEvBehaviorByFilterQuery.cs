using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaTrouble.Get.ListEv
{
    class SdaTroubleGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaTroubleGetListEv
    {
        SdaTroubleFilterQuery filterQuery;

        internal SdaTroubleGetListEvBehaviorByFilterQuery(CommonParam param, SdaTroubleFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_TROUBLE> ISdaTroubleGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaTroubleDAO.Get(filterQuery.Query(), param);
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
