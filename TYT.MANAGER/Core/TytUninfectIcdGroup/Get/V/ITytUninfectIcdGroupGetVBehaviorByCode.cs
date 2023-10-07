using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.V
{
    class TytUninfectIcdGroupGetVBehaviorByCode : BeanObjectBase, ITytUninfectIcdGroupGetV
    {
        string code;

        internal TytUninfectIcdGroupGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_UNINFECT_ICD_GROUP ITytUninfectIcdGroupGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.GetViewByCode(code, new TytUninfectIcdGroupViewFilterQuery().Query());
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
