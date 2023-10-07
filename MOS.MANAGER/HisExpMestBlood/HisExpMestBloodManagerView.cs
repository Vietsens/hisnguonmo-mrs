using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBlood
{
    public partial class HisExpMestBloodManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_BLOOD> GetView(HisExpMestBloodViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_BLOOD GetViewById(long data)
        {
            V_HIS_EXP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_BLOOD GetViewById(long data, HisExpMestBloodViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_EXP_MEST_BLOOD> GetViewByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_BLOOD>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData = new HisExpMestBloodGet(param).GetViewByExpMestIds(Ids);
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

        
        public List<V_HIS_EXP_MEST_BLOOD> GetViewByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetViewByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_BLOOD> GetViewByAggrExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetViewByAggrExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_BLOOD> GetViewUnexportByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBloodGet(param).GetViewUnexportByExpMestId(data);
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
