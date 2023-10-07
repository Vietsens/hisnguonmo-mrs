using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineType
{
    public partial class HisMedicineTypeManager : BusinessBase
    {
        
        public List<V_HIS_MEDICINE_TYPE> GetView(HisMedicineTypeViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetView(filter);
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

        
        public V_HIS_MEDICINE_TYPE GetViewByCode(string data)
        {
            V_HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetViewByCode(data);
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

        
        public V_HIS_MEDICINE_TYPE GetViewByCode(string data, HisMedicineTypeViewFilterQuery filter)
        {
            V_HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_MEDICINE_TYPE GetViewById(long data)
        {
            V_HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetViewById(data);
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

        
        public V_HIS_MEDICINE_TYPE GetViewById(long data, HisMedicineTypeViewFilterQuery filter)
        {
            V_HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_MEDICINE_TYPE> GetViewByIds(List<long> data)
        {
            List<V_HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MEDICINE_TYPE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineTypeGet(param).GetViewByIds(Ids));
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
