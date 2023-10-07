using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytUninfectIcd.Get.Ev
{
    class TytUninfectIcdGetEvBehaviorByCode : BeanObjectBase, ITytUninfectIcdGetEv
    {
        string code;

        internal TytUninfectIcdGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        TYT_UNINFECT_ICD ITytUninfectIcdGetEv.Run()
        {
            try
            {
                return DAOWorker.TytUninfectIcdDAO.GetByCode(code, new TytUninfectIcdFilterQuery().Query());
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
