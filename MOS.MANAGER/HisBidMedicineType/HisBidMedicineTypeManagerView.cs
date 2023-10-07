using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMedicineType
{
    public partial class HisBidMedicineTypeManager : BusinessBase
    {
        
        public List<V_HIS_BID_MEDICINE_TYPE> GetView(HisBidMedicineTypeViewFilterQuery filter)
        {
            List<V_HIS_BID_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BID_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).GetView(filter);
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

        
        public V_HIS_BID_MEDICINE_TYPE GetViewById(long data)
        {
            V_HIS_BID_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BID_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).GetViewById(data);
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

        
        public V_HIS_BID_MEDICINE_TYPE GetViewById(long data, HisBidMedicineTypeViewFilterQuery filter)
        {
            V_HIS_BID_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BID_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).GetViewById(data, filter);
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
