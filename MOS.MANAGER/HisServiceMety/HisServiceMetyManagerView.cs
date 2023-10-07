using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMety
{
    public partial class HisServiceMetyManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_METY> GetView(HisServiceMetyViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).GetView(filter);
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
