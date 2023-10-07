using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfect.Get.Ev
{
    class TytUninfectGetEvBehaviorById : BeanObjectBase, ITytUninfectGetEv
    {
        long id;

        internal TytUninfectGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_UNINFECT ITytUninfectGetEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectDAO.GetById(id, new TytUninfectFilterQuery().Query());
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
