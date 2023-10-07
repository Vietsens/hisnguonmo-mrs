using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroup.Get.Ev
{
    class SdaGroupGetEvBehaviorByCode : BeanObjectBase, ISdaGroupGetEv
    {
        string code;

        internal SdaGroupGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_GROUP ISdaGroupGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupDAO.GetByCode(code, new SdaGroupFilterQuery().Query());
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
