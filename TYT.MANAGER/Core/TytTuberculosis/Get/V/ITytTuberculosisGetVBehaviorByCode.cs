using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.V
{
    class TytTuberculosisGetVBehaviorByCode : BeanObjectBase, ITytTuberculosisGetV
    {
        string code;

        internal TytTuberculosisGetVBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        V_TYT_TUBERCULOSIS ITytTuberculosisGetV.Run()
        {
            try
            {
                return DAOWorker.TytTuberculosisDAO.GetViewByCode(code, new TytTuberculosisViewFilterQuery().Query());
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
