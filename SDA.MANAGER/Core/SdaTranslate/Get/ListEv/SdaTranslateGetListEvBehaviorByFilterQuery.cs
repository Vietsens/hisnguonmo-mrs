using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaTranslate.Get.ListEv
{
    class SdaTranslateGetListEvBehaviorByFilterQuery : BeanObjectBase, ISdaTranslateGetListEv
    {
        SdaTranslateFilterQuery filterQuery;

        internal SdaTranslateGetListEvBehaviorByFilterQuery(CommonParam param, SdaTranslateFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<SDA_TRANSLATE> ISdaTranslateGetListEv.Run()
        {
            try
            {
                return DAOWorker.SdaTranslateDAO.Get(filterQuery.Query(), param);
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
