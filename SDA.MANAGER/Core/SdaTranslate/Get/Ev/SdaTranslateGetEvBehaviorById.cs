using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTranslate.Get.Ev
{
    class SdaTranslateGetEvBehaviorById : BeanObjectBase, ISdaTranslateGetEv
    {
        long id;

        internal SdaTranslateGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_TRANSLATE ISdaTranslateGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaTranslateDAO.GetById(id, new SdaTranslateFilterQuery().Query());
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
