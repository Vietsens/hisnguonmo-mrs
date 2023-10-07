using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    public partial class HisMaterialBeanManager : BusinessBase
    {
        
        public List<V_HIS_MATERIAL_BEAN> GetView(HisMaterialBeanViewFilterQuery filter)
        {
            List<V_HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetView(filter);
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

        
        public V_HIS_MATERIAL_BEAN GetViewById(long data)
        {
            V_HIS_MATERIAL_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MATERIAL_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetViewById(data);
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

        
        public V_HIS_MATERIAL_BEAN GetViewById(long data, HisMaterialBeanViewFilterQuery filter)
        {
            V_HIS_MATERIAL_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MATERIAL_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_MATERIAL_BEAN> GetViewByIds(List<long> data)
        {
            List<V_HIS_MATERIAL_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MATERIAL_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MATERIAL_BEAN>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialBeanGet(param).GetViewByIds(Ids));
                    }
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
