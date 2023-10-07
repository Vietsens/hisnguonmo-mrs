using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMedicineType
{
    public partial class HisEmteMedicineTypeManager : BusinessBase
    {
        
        public List<V_HIS_EMTE_MEDICINE_TYPE> GetView(HisEmteMedicineTypeViewFilterQuery filter)
        {
            List<V_HIS_EMTE_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EMTE_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetView(filter);
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

        
        public V_HIS_EMTE_MEDICINE_TYPE GetViewById(long data)
        {
            V_HIS_EMTE_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EMTE_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetViewById(data);
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

        
        public V_HIS_EMTE_MEDICINE_TYPE GetViewById(long data, HisEmteMedicineTypeViewFilterQuery filter)
        {
            V_HIS_EMTE_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EMTE_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMedicineTypeGet(param).GetViewById(data, filter);
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
