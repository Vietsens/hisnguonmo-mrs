using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytGdsk.Get.Ev
{
    class TytGdskGetEvBehaviorById : BeanObjectBase, ITytGdskGetEv
    {
        long id;

        internal TytGdskGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_GDSK ITytGdskGetEv.Run()
        {
            try
            {
                return DAOWorker.TytGdskDAO.GetById(id, new TytGdskFilterQuery().Query());
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
