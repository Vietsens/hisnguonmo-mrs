using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytMalaria.Get.Ev
{
    class TytMalariaGetEvBehaviorById : BeanObjectBase, ITytMalariaGetEv
    {
        long id;

        internal TytMalariaGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_MALARIA ITytMalariaGetEv.Run()
        {
            try
            {
                return DAOWorker.TytMalariaDAO.GetById(id, new TytMalariaFilterQuery().Query());
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
