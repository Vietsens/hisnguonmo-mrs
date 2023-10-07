using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytDeath.Get.Ev
{
    class TytDeathGetEvBehaviorById : BeanObjectBase, ITytDeathGetEv
    {
        long id;

        internal TytDeathGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_DEATH ITytDeathGetEv.Run()
        {
            try
            {
                return DAOWorker.TytDeathDAO.GetById(id, new TytDeathFilterQuery().Query());
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
