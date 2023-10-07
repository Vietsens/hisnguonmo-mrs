using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public partial class HisMedicineBeanManager : BusinessBase
    {
        
        public List<V_HIS_MEDICINE_BEAN> GetView(HisMedicineBeanViewFilterQuery filter)
        {
            List<V_HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetView(filter);
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

        
        public V_HIS_MEDICINE_BEAN GetViewById(long data)
        {
            V_HIS_MEDICINE_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetViewById(data);
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

        
        public V_HIS_MEDICINE_BEAN GetViewById(long data, HisMedicineBeanViewFilterQuery filter)
        {
            V_HIS_MEDICINE_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_MEDICINE_BEAN> GetViewByIds(List<long> data)
        {
            List<V_HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MEDICINE_BEAN>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineBeanGet(param).GetViewByIds(Ids));
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

        
        public List<V_HIS_MEDICINE_BEAN_1> GetView(HisMedicineBeanView1FilterQuery filter)
        {
            List<V_HIS_MEDICINE_BEAN_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_BEAN_1> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetView1(filter);
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

        
        public V_HIS_MEDICINE_BEAN_1 GetView1ById(long data)
        {
            V_HIS_MEDICINE_BEAN_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_BEAN_1 resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetView1ById(data);
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

        
        public V_HIS_MEDICINE_BEAN_1 GetView1ById(long data, HisMedicineBeanView1FilterQuery filter)
        {
            V_HIS_MEDICINE_BEAN_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_BEAN_1 resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetView1ById(data, filter);
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

        
        public List<V_HIS_MEDICINE_BEAN_1> GetView1ByIds(List<long> data)
        {
            List<V_HIS_MEDICINE_BEAN_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MEDICINE_BEAN_1> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MEDICINE_BEAN_1>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineBeanGet(param).GetView1ByIds(Ids));
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
