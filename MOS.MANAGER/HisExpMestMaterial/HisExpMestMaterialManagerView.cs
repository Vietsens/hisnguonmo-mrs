using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMaterial
{
    public partial class HisExpMestMaterialManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_MATERIAL> GetView(HisExpMestMaterialViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_MATERIAL GetViewById(long data)
        {
            V_HIS_EXP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_MATERIAL GetViewById(long data, HisExpMestMaterialViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewByTreatmentId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewByTreatmentId(data);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MATERIAL>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMaterialGet(param).GetViewByExpMestIds(Ids));
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewByIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MATERIAL>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMaterialGet(param).GetViewByIds(Ids));
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewRequestByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewRequestByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewExecuteByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewExecuteByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL> GetViewByAggrExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetViewByAggrExpMestId(data);
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
