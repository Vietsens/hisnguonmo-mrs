using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCoTreatment
{
    public partial class HisCoTreatmentManager : BusinessBase
    {
        public HisCoTreatmentManager()
            : base()
        {

        }

        public HisCoTreatmentManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_CO_TREATMENT> Get(HisCoTreatmentFilterQuery filter)
        {
            List<HIS_CO_TREATMENT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisCoTreatmentGet(param).Get(filter);
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
