using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMaterialType
{
    public partial class HisEmteMaterialTypeManager : BusinessBase
    {
        
        public List<V_HIS_EMTE_MATERIAL_TYPE> GetView(HisEmteMaterialTypeViewFilterQuery filter)
        {
            List<V_HIS_EMTE_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EMTE_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetView(filter);
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

        
        public V_HIS_EMTE_MATERIAL_TYPE GetViewById(long data)
        {
            V_HIS_EMTE_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EMTE_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetViewById(data);
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

        
        public V_HIS_EMTE_MATERIAL_TYPE GetViewById(long data, HisEmteMaterialTypeViewFilterQuery filter)
        {
            V_HIS_EMTE_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EMTE_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisEmteMaterialTypeGet(param).GetViewById(data, filter);
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
