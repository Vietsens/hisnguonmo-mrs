using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCoTreatment
{
    public partial class HisCoTreatmentManager : BusinessBase
    {
        public List<V_HIS_CO_TREATMENT> GetView(HisCoTreatmentViewFilterQuery filter)
        {
            List<V_HIS_CO_TREATMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisCoTreatmentGet(param).GetView(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
