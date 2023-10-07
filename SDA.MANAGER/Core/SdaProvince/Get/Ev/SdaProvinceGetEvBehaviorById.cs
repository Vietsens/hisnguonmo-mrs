using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaProvince.Get.Ev
{
    class SdaProvinceGetEvBehaviorById : BeanObjectBase, ISdaProvinceGetEv
    {
        long id;

        internal SdaProvinceGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_PROVINCE ISdaProvinceGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaProvinceDAO.GetById(id, new SdaProvinceFilterQuery().Query());
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
