using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaReligion.Get.Ev
{
    class SdaReligionGetEvBehaviorByCode : BeanObjectBase, ISdaReligionGetEv
    {
        string code;

        internal SdaReligionGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_RELIGION ISdaReligionGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaReligionDAO.GetByCode(code, new SdaReligionFilterQuery().Query());
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
