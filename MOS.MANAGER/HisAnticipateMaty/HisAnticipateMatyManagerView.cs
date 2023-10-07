using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMaty
{
    public partial class HisAnticipateMatyManager : BusinessBase
    {
        
        public List<V_HIS_ANTICIPATE_MATY> GetView(HisAnticipateMatyViewFilterQuery filter)
        {
            List<V_HIS_ANTICIPATE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_ANTICIPATE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).GetView(filter);
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

        
        public V_HIS_ANTICIPATE_MATY GetViewById(long data)
        {
            V_HIS_ANTICIPATE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_ANTICIPATE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).GetViewById(data);
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

        
        public V_HIS_ANTICIPATE_MATY GetViewById(long data, HisAnticipateMatyViewFilterQuery filter)
        {
            V_HIS_ANTICIPATE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_ANTICIPATE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisAnticipateMatyGet(param).GetViewById(data, filter);
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
