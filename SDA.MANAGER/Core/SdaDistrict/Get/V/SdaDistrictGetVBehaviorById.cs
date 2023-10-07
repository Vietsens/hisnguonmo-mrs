using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaDistrict.Get.V
{
    class SdaDistrictGetVBehaviorById : BeanObjectBase, ISdaDistrictGetV
    {
        long id;

        internal SdaDistrictGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_SDA_DISTRICT ISdaDistrictGetV.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.GetViewById(id, new SdaDistrictViewFilterQuery().Query());
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
