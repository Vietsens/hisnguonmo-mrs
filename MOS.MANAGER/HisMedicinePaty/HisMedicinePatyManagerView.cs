using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicinePaty
{
    public partial class HisMedicinePatyManager : BusinessBase
    {
        
        public List<V_HIS_MEDICINE_PATY> GetView(HisMedicinePatyViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetView(filter);
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

        
        public V_HIS_MEDICINE_PATY GetViewById(long data)
        {
            V_HIS_MEDICINE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetViewById(data);
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

        
        public V_HIS_MEDICINE_PATY GetViewById(long data, HisMedicinePatyViewFilterQuery filter)
        {
            V_HIS_MEDICINE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMedicinePatyGet(param).GetViewById(data, filter);
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
