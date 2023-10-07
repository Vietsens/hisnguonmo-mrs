using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaLanguage.Get.Ev
{
    class SdaLanguageGetEvBehaviorById : BeanObjectBase, ISdaLanguageGetEv
    {
        long id;

        internal SdaLanguageGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        SDA_LANGUAGE ISdaLanguageGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaLanguageDAO.GetById(id, new SdaLanguageFilterQuery().Query());
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
