using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMest
{
    public partial class HisImpMestManager : BusinessBase
    {
        
        public List<V_HIS_IMP_MEST> GetView(HisImpMestViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView(filter);
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

        
        public V_HIS_IMP_MEST GetViewByCode(string data)
        {
            V_HIS_IMP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetViewByCode(data);
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

        
        public V_HIS_IMP_MEST GetViewByCode(string data, HisImpMestViewFilterQuery filter)
        {
            V_HIS_IMP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_IMP_MEST GetViewById(long data)
        {
            V_HIS_IMP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetViewById(data);
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

        
        public V_HIS_IMP_MEST GetViewById(long data, HisImpMestViewFilterQuery filter)
        {
            V_HIS_IMP_MEST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_IMP_MEST> GetViewByIds(List<long> data)
        {
            List<V_HIS_IMP_MEST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_IMP_MEST>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestGet(param).GetViewByIds(Ids));
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

        
        public List<V_HIS_IMP_MEST_MANU> GetManuView(HisImpMestManuViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST_MANU> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_MANU> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetManuView(filter);
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

        
        public V_HIS_IMP_MEST_MANU GetManuViewByCode(string data)
        {
            V_HIS_IMP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MANU resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetManuViewByCode(data);
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

        
        public V_HIS_IMP_MEST_MANU GetManuViewByCode(string data, HisImpMestManuViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MANU resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetManuViewByCode(data, filter);
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

        
        public V_HIS_IMP_MEST_MANU GetManuViewById(long data)
        {
            V_HIS_IMP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MANU resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetManuViewById(data);
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

        
        public V_HIS_IMP_MEST_MANU GetManuViewById(long data, HisImpMestManuViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_MANU result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MANU resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetManuViewById(data, filter);
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

        
        public List<V_HIS_IMP_MEST_1> GetView1(HisImpMestView1FilterQuery filter)
        {
            List<V_HIS_IMP_MEST_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_1> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView1(filter);
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

        
        public V_HIS_IMP_MEST_1 GetView1ByCode(string data)
        {
            V_HIS_IMP_MEST_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_1 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView1ByCode(data);
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

        
        public V_HIS_IMP_MEST_1 GetView1ByCode(string data, HisImpMestView1FilterQuery filter)
        {
            V_HIS_IMP_MEST_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_1 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView1ByCode(data, filter);
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

        
        public V_HIS_IMP_MEST_1 GetView1ById(long data)
        {
            V_HIS_IMP_MEST_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_1 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView1ById(data);
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

        
        public V_HIS_IMP_MEST_1 GetView1ById(long data, HisImpMestView1FilterQuery filter)
        {
            V_HIS_IMP_MEST_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_1 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestGet(param).GetView1ById(data, filter);
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
