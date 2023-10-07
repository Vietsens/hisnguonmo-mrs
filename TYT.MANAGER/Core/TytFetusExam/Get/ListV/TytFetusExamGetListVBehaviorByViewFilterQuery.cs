using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.MANAGER.Core.TytFetusExam.Get.ListV
{
    class TytFetusExamGetListVBehaviorByViewFilterQuery : BeanObjectBase, ITytFetusExamGetListV
    {
        TytFetusExamViewFilterQuery filterQuery;

        internal TytFetusExamGetListVBehaviorByViewFilterQuery(CommonParam param, TytFetusExamViewFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        List<V_TYT_FETUS_EXAM> ITytFetusExamGetListV.Run()
        {
            try
            {
                return DAOWorker.TytFetusExamDAO.GetView(filterQuery.Query(), param);
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
