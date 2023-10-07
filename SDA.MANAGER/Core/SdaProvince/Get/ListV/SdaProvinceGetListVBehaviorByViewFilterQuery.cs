using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaProvince.Get.ListV
{
    class SdaProvinceGetListVBehaviorByViewFilterQuery : BeanObjectBase, ISdaProvinceGetListV
    {
        SdaProvinceViewFilterQuery filterQuery;

        internal SdaProvinceGetListVBehaviorByViewFilterQuery(CommonParam param, SdaProvinceViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_SDA_PROVINCE> ISdaProvinceGetListV.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.GetView(filterQuery.Query(), param);
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
