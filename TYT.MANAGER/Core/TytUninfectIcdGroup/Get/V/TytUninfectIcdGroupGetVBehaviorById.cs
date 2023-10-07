using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.V
{
    class TytUninfectIcdGroupGetVBehaviorById : BeanObjectBase, ITytUninfectIcdGroupGetV
    {
        long id;

        internal TytUninfectIcdGroupGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_UNINFECT_ICD_GROUP ITytUninfectIcdGroupGetV.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.GetViewById(id, new TytUninfectIcdGroupViewFilterQuery().Query());
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
