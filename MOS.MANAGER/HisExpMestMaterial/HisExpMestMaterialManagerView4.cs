using Inventec.Common.Logging;
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
        
        public List<V_HIS_EXP_MEST_MATERIAL_4> GetView4(HisExpMestMaterialView4FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MATERIAL_4> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_MATERIAL_4> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetView4(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }

        
        public V_HIS_EXP_MEST_MATERIAL_4 GetView4ById(long data)
        {
            V_HIS_EXP_MEST_MATERIAL_4 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_MATERIAL_4 resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetView4ById(data);
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

        
        public V_HIS_EXP_MEST_MATERIAL_4 GetView4ById(long data, HisExpMestMaterialView4FilterQuery filter)
        {
            V_HIS_EXP_MEST_MATERIAL_4 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_MATERIAL_4 resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetView4ById(data, filter);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MATERIAL_4> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL_4> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MATERIAL_4>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMaterialGet(param).GetView4ByIds(Ids));
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

        
        public List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MATERIAL_4> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL_4> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMaterialGet(param).GetView4ByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MATERIAL_4> GetView4ByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MATERIAL_4> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MATERIAL_4> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MATERIAL_4>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMaterialGet(param).GetView4ByExpMestIds(Ids));
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
