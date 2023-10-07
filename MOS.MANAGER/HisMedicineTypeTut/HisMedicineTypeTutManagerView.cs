using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeTut
{
    public partial class HisMedicineTypeTutManager : BusinessBase
    {
        
        public List<V_HIS_MEDICINE_TYPE_TUT> GetView(HisMedicineTypeTutViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE_TYPE_TUT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_TYPE_TUT> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).GetView(filter);
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

        
        public V_HIS_MEDICINE_TYPE_TUT GetViewById(long data)
        {
            V_HIS_MEDICINE_TYPE_TUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_TYPE_TUT resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).GetViewById(data);
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

        
        public V_HIS_MEDICINE_TYPE_TUT GetViewById(long data, HisMedicineTypeTutViewFilterQuery filter)
        {
            V_HIS_MEDICINE_TYPE_TUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_TYPE_TUT resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).GetViewById(data, filter);
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
