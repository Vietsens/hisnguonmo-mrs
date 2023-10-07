using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaTranslate.Get.Ev
{
    class SdaTranslateGetEvBehaviorByCode : BeanObjectBase, ISdaTranslateGetEv
    {
        string code;

        internal SdaTranslateGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_TRANSLATE ISdaTranslateGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaTranslateDAO.GetByCode(code, new SdaTranslateFilterQuery().Query());
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
