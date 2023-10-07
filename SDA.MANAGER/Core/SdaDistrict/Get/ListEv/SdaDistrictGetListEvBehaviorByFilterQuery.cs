using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict.Get.ListEv
{
    class SdaDistrictGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaDistrictGetListEv
    {
        SdaDistrictFilterQuery filterQuery;

        internal SdaDistrictGetListEvBehaviorByFilterQuery(CommonParam param, SdaDistrictFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_DISTRICT> ISdaDistrictGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.Get(filterQuery.Query(), param);
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
