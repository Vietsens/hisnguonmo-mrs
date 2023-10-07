using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestMetyDepa
{
    public partial class HisMestMetyDepaManager : BusinessBase
    {
        
        public List<V_HIS_MEST_METY_DEPA> GetView(HisMestMetyDepaViewFilterQuery filter)
        {
            List<V_HIS_MEST_METY_DEPA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_METY_DEPA> resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).GetView(filter);
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

        
        public V_HIS_MEST_METY_DEPA GetViewById(long data)
        {
            V_HIS_MEST_METY_DEPA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_METY_DEPA resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_METY_DEPA GetViewById(long data, HisMestMetyDepaViewFilterQuery filter)
        {
            V_HIS_MEST_METY_DEPA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_METY_DEPA resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).GetViewById(data, filter);
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
