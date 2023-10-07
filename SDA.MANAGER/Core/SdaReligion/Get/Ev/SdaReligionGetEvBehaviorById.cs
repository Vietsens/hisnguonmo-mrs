using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaReligion.Get.Ev
{
    class SdaReligionGetEvBehaviorById : BeanObjectBase, ISdaReligionGetEv
    {
        long id;

        internal SdaReligionGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_RELIGION ISdaReligionGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaReligionDAO.GetById(id, new SdaReligionFilterQuery().Query());
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
