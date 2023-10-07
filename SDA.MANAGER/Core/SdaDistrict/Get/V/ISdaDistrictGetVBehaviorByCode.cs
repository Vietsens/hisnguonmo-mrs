using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaDistrict.Get.V
{
    class SdaDistrictGetVBehaviorByCode : BeanObjectBase, ISdaDistrictGetV
    {
        string code;

        internal SdaDistrictGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_SDA_DISTRICT ISdaDistrictGetV.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.GetViewByCode(code, new SdaDistrictViewFilterQuery().Query());
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
