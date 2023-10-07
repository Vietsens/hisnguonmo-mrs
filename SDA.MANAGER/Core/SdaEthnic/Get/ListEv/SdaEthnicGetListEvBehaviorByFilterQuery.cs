using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaEthnic.Get.ListEv
{
    class SdaEthnicGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaEthnicGetListEv
    {
        SdaEthnicFilterQuery filterQuery;

        internal SdaEthnicGetListEvBehaviorByFilterQuery(CommonParam param, SdaEthnicFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_ETHNIC> ISdaEthnicGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaEthnicDAO.Get(filterQuery.Query(), param);
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
