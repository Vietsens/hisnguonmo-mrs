using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMaty
{
    public partial class HisServiceMatyManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_MATY> GetView(HisServiceMatyViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).GetView(filter);
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
