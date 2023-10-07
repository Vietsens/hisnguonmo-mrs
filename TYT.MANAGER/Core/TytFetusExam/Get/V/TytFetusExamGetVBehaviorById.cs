using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusExam.Get.V
{
    class TytFetusExamGetVBehaviorById : BeanObjectBase, ITytFetusExamGetV
    {
        long id;

        internal TytFetusExamGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_FETUS_EXAM ITytFetusExamGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusExamDAO.GetViewById(id, new TytFetusExamViewFilterQuery().Query());
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
