using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLanguage.Get.Ev
{
    class SdaLanguageGetEvBehaviorByCode : BeanObjectBase, ISdaLanguageGetEv
    {
        string code;

        internal SdaLanguageGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_LANGUAGE ISdaLanguageGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaLanguageDAO.GetByCode(code, new SdaLanguageFilterQuery().Query());
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
