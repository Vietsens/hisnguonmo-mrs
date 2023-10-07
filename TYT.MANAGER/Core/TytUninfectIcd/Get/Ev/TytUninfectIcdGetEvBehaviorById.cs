using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.Ev
{
    class TytUninfectIcdGetEvBehaviorById : BeanObjectBase, ITytUninfectIcdGetEv
    {
        long id;

        internal TytUninfectIcdGetEvBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        TYT_UNINFECT_ICD ITytUninfectIcdGetEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.GetById(id, new TytUninfectIcdFilterQuery().Query());
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
