using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaDistrict.Get.Ev
{
    class SdaDistrictGetEvBehaviorById : BeanObjectBase, ISdaDistrictGetEv
    {
        long id;

        internal SdaDistrictGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_DISTRICT ISdaDistrictGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaDistrictDAO.GetById(id, new SdaDistrictFilterQuery().Query());
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
