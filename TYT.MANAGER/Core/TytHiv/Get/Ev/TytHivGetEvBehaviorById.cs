using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytHiv.Get.Ev
{
    class TytHivGetEvBehaviorById : BeanObjectBase, ITytHivGetEv
    {
        long id;

        internal TytHivGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_HIV ITytHivGetEv.Run()
        {
            try
            {
                return DAOWorker.TytHivDAO.GetById(id, new TytHivFilterQuery().Query());
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
