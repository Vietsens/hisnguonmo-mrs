using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRestRetrType
{
    public partial class HisRestRetrTypeManager : BusinessBase
    {
        
        public List<V_HIS_REST_RETR_TYPE> GetView(HisRestRetrTypeViewFilterQuery filter)
        {
            List<V_HIS_REST_RETR_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_REST_RETR_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).GetView(filter);
                }
                result = resultData;
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
