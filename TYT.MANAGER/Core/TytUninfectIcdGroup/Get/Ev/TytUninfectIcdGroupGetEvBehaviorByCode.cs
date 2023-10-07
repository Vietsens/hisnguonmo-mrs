using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.Ev
{
    class TytUninfectIcdGroupGetEvBehaviorByCode : BeanObjectBase, ITytUninfectIcdGroupGetEv
    {
        string code;

        internal TytUninfectIcdGroupGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        TYT_UNINFECT_ICD_GROUP ITytUninfectIcdGroupGetEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.GetByCode(code, new TytUninfectIcdGroupFilterQuery().Query());
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
