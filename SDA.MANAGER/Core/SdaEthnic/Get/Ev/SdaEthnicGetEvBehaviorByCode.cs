using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaEthnic.Get.Ev
{
    class SdaEthnicGetEvBehaviorByCode : BeanObjectBase, ISdaEthnicGetEv
    {
        string code;

        internal SdaEthnicGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_ETHNIC ISdaEthnicGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaEthnicDAO.GetByCode(code, new SdaEthnicFilterQuery().Query());
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
