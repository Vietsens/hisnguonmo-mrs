using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaDistrict.Get.Ev
{
    class SdaDistrictGetEvBehaviorByCode : BeanObjectBase, ISdaDistrictGetEv
    {
        string code;

        internal SdaDistrictGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_DISTRICT ISdaDistrictGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.GetByCode(code, new SdaDistrictFilterQuery().Query());
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
