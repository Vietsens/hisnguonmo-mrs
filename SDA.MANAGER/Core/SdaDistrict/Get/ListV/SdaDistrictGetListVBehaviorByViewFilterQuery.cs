using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict.Get.ListV
{
    class SdaDistrictGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaDistrictGetListV
    {
        SdaDistrictViewFilterQuery filterQuery;

        internal SdaDistrictGetListVBehaviorByViewFilterQuery(CommonParam param, SdaDistrictViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_DISTRICT> ISdaDistrictGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.GetView(filterQuery.Query(), param);
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
