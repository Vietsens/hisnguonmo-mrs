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
        
        public List<V_HIS_MATERIAL_BEAN_2> GetView2(HisMaterialBeanView2FilterQuery filter)
        {
            List<V_HIS_MATERIAL_BEAN_2> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MATERIAL_BEAN_2> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetView2(filter);
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

        
        public V_HIS_MATERIAL_BEAN_2 GetView2ById(long data)
        {
            V_HIS_MATERIAL_BEAN_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MATERIAL_BEAN_2 resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetView2ById(data);
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

        
        public V_HIS_MATERIAL_BEAN_2 GetView2ById(long data, HisMaterialBeanView2FilterQuery filter)
        {
            V_HIS_MATERIAL_BEAN_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MATERIAL_BEAN_2 resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialBeanGet(param).GetView2ById(data, filter);
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

        
        public List<V_HIS_MATERIAL_BEAN_2> GetView2ByIds(List<long> data)
        {
            List<V_HIS_MATERIAL_BEAN_2> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MATERIAL_BEAN_2> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MATERIAL_BEAN_2>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialBeanGet(param).GetView2ByIds(Ids));
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
