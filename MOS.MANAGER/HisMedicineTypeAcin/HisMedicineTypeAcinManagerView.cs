using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeAcin
{
    public partial class HisMedicineTypeAcinManager : BusinessBase
    {
        
        public List<V_HIS_MEDICINE_TYPE_ACIN> GetView(HisMedicineTypeAcinViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE_TYPE_ACIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_TYPE_ACIN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetView(filter);
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

        
        public V_HIS_MEDICINE_TYPE_ACIN GetViewById(long data)
        {
            V_HIS_MEDICINE_TYPE_ACIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_TYPE_ACIN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetViewById(data);
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

        
        public V_HIS_MEDICINE_TYPE_ACIN GetViewById(long data, HisMedicineTypeAcinViewFilterQuery filter)
        {
            V_HIS_MEDICINE_TYPE_ACIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_TYPE_ACIN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetViewById(data, filter);
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
