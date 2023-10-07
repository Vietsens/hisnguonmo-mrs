using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcdGroup.Get.Ev
{
    class TytUninfectIcdGroupGetEvBehaviorById : BeanObjectBase, ITytUninfectIcdGroupGetEv
    {
        long id;

        internal TytUninfectIcdGroupGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_UNINFECT_ICD_GROUP ITytUninfectIcdGroupGetEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdGroupDAO.GetById(id, new TytUninfectIcdGroupFilterQuery().Query());
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
