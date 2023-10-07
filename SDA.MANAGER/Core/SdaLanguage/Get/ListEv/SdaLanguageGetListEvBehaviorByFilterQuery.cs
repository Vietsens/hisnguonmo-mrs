using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLanguage.Get.ListEv
{
    class SdaLanguageGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaLanguageGetListEv
    {
        SdaLanguageFilterQuery filterQuery;

        internal SdaLanguageGetListEvBehaviorByFilterQuery(CommonParam param, SdaLanguageFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_LANGUAGE> ISdaLanguageGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaLanguageDAO.Get(filterQuery.Query(), param);
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
