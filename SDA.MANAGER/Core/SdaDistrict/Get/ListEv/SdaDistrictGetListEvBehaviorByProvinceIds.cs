using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaDistrict.Get.ListEv
{
    class SdaDistrictGetListEvBehaviorByProvinceIds : BeanObjectBase, ISdaDistrictGetListEv
    {
        List<long> provinceIds;

        internal SdaDistrictGetListEvBehaviorByProvinceIds(CommonParam param, List<long> provinceids)
            : base(param)
        {
            provinceIds = provinceids;
        }

        List<SDA_DISTRICT> ISdaDistrictGetListEv.Run()
        {
            try
            {
                SdaDistrictFilterQuery filter = new SdaDistrictFilterQuery();
                filter.PROVINCE_IDs = provinceIds;
                return DAOWorker.SdaDistrictDAO.Get(filter.Query(), param);
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
