using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytNerves.Get.V
{
    class TytNervesGetVBehaviorByCode : BeanObjectBase, ITytNervesGetV
    {
        string code;

        internal TytNervesGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_NERVES ITytNervesGetV.Run()
        {
            try
            {
                return DAOWorker.TytNervesDAO.GetViewByCode(code, new TytNervesViewFilterQuery().Query());
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
