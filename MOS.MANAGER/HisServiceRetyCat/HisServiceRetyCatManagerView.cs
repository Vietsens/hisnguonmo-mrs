using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRetyCat
{
    public partial class HisServiceRetyCatManager : BusinessBase
    {
        
        public List<V_HIS_SERVICE_RETY_CAT> GetView(HisServiceRetyCatViewFilterQuery filter)
        {
            List<V_HIS_SERVICE_RETY_CAT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_RETY_CAT> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRetyCatGet(param).GetView(filter);
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
