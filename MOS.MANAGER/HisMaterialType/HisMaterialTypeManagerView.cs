using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialType
{
    public partial class HisMaterialTypeManager : BusinessBase
    {
        
        public List<V_HIS_MATERIAL_TYPE> GetView(HisMaterialTypeViewFilterQuery filter)
        {
            List<V_HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetView(filter);
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

        
        public V_HIS_MATERIAL_TYPE GetViewByCode(string data)
        {
            V_HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetViewByCode(data);
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

        
        public V_HIS_MATERIAL_TYPE GetViewByCode(string data, HisMaterialTypeViewFilterQuery filter)
        {
            V_HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_MATERIAL_TYPE GetViewById(long data)
        {
            V_HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetViewById(data);
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

        
        public V_HIS_MATERIAL_TYPE GetViewById(long data, HisMaterialTypeViewFilterQuery filter)
        {
            V_HIS_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMaterialTypeGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_MATERIAL_TYPE> GetViewByIds(List<long> data)
        {
            List<V_HIS_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MATERIAL_TYPE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMaterialTypeGet(param).GetViewByIds(Ids));
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
