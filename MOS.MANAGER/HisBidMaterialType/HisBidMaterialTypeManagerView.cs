using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMaterialType
{
    public partial class HisBidMaterialTypeManager : BusinessBase
    {
        
        public List<V_HIS_BID_MATERIAL_TYPE> GetView(HisBidMaterialTypeViewFilterQuery filter)
        {
            List<V_HIS_BID_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BID_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).GetView(filter);
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

        
        public V_HIS_BID_MATERIAL_TYPE GetViewById(long data)
        {
            V_HIS_BID_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BID_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).GetViewById(data);
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

        
        public V_HIS_BID_MATERIAL_TYPE GetViewById(long data, HisBidMaterialTypeViewFilterQuery filter)
        {
            V_HIS_BID_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BID_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).GetViewById(data, filter);
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
