using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainType
{
    public partial class HisRehaTrainTypeManager : BusinessBase
    {
        
        public List<V_HIS_REHA_TRAIN_TYPE> GetView(HisRehaTrainTypeViewFilterQuery filter)
        {
            List<V_HIS_REHA_TRAIN_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_REHA_TRAIN_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainTypeGet(param).GetView(filter);
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
