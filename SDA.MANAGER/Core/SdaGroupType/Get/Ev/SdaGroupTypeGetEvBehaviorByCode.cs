using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;

namespace SDA.MANAGER.Core.SdaGroupType.Get.Ev
{
    class SdaGroupTypeGetEvBehaviorByCode : BeanObjectBase, ISdaGroupTypeGetEv
    {
        string code;

        internal SdaGroupTypeGetEvBehaviorByCode(CommonParam param, string filter)
            : base(param)
        {
            code = filter;
        }

        SDA_GROUP_TYPE ISdaGroupTypeGetEv.Run()
        {
            try
            {
                return DAOWorker.SdaGroupTypeDAO.GetByCode(code, new SdaGroupTypeFilterQuery().Query());
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
