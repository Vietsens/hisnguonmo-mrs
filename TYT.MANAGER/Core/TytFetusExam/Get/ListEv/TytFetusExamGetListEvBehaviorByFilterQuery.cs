using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusExam.Get.ListEv
{
    class TytFetusExamGetListEvBehaviorByFilterQuery : BeanObjectBase, ITytFetusExamGetListEv
    {
        TytFetusExamFilterQuery filterQuery;

        internal TytFetusExamGetListEvBehaviorByFilterQuery(CommonParam param, TytFetusExamFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<TYT_FETUS_EXAM> ITytFetusExamGetListEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusExamDAO.Get(filterQuery.Query(), param);
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
