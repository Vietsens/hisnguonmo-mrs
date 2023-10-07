using TYT.EFMODEL.DataModels;
using TYT.MANAGER.Base;
using Inventec.Core;
using System;

namespace TYT.MANAGER.Core.TytTuberculosis.Get.V
{
    class TytTuberculosisGetVBehaviorById : BeanObjectBase, ITytTuberculosisGetV
    {
        long id;

        internal TytTuberculosisGetVBehaviorById(CommonParam param, long filter)
            : base(param)
        {
            id = filter;
        }

        V_TYT_TUBERCULOSIS ITytTuberculosisGetV.Run()
        {
            try
            {
                return DAOWorker.TytTuberculosisDAO.GetViewById(id, new TytTuberculosisViewFilterQuery().Query());
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
