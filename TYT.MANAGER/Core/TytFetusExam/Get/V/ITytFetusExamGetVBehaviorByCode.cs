using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusExam.Get.V
{
    class TytFetusExamGetVBehaviorByCode : BeanObjectBase, ITytFetusExamGetV
    {
        string code;

        internal TytFetusExamGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_FETUS_EXAM ITytFetusExamGetV.Run()
        {
            try
            {
                return DAOWorker.TytFetusExamDAO.GetViewByCode(code, new TytFetusExamViewFilterQuery().Query());
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
