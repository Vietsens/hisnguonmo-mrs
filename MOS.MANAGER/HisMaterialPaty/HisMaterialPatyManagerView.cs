using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialPaty
{
    public partial class HisMaterialPatyManager : BusinessBase
    {
        
        public List<V_HIS_MATERIAL_PATY> GetView(HisMaterialPatyViewFilterQuery filter)
        {
            List<V_HIS_MATERIAL_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MATERIAL_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetView(filter);
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

        
        public V_HIS_MATERIAL_PATY GetViewById(long data)
        {
            V_HIS_MATERIAL_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MATERIAL_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetViewById(data);
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

        
        public V_HIS_MATERIAL_PATY GetViewById(long data, HisMaterialPatyViewFilterQuery filter)
        {
            V_HIS_MATERIAL_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MATERIAL_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialPatyGet(param).GetViewById(data, filter);
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
