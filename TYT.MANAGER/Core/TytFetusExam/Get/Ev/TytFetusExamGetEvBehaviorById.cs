using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusExam.Get.Ev
{
    class TytFetusExamGetEvBehaviorById : BeanObjectBase, ITytFetusExamGetEv
    {
        long id;

        internal TytFetusExamGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_FETUS_EXAM ITytFetusExamGetEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusExamDAO.GetById(id, new TytFetusExamFilterQuery().Query());
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
