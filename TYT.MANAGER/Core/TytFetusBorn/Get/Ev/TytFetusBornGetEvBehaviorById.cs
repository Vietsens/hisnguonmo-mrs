using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytFetusBorn.Get.Ev
{
    class TytFetusBornGetEvBehaviorById : BeanObjectBase, ITytFetusBornGetEv
    {
        long id;

        internal TytFetusBornGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_FETUS_BORN ITytFetusBornGetEv.Run()
        {
            try
            {
                return DAOWorker.TytFetusBornDAO.GetById(id, new TytFetusBornFilterQuery().Query());
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
