using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMaterial
{
    public partial class HisImpMestMaterialManager : BusinessBase
    {
        
        public List<V_HIS_IMP_MEST_MATERIAL> GetView(HisImpMestMaterialViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView(filter);
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

        
        public V_HIS_IMP_MEST_MATERIAL GetViewById(long data)
        {
            V_HIS_IMP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewById(data);
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

        
        public V_HIS_IMP_MEST_MATERIAL GetViewById(long data, HisImpMestMaterialViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_MATERIAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MATERIAL resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_IMP_MEST_MATERIAL> GetViewByAggrImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewByAggrImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MATERIAL> GetViewByAggrImpMestIdAndGroupByMaterial(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewByAggrImpMestIdAndGroupByMaterial(data);
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

        
        public List<V_HIS_IMP_MEST_MATERIAL> GetViewAndIncludeChildrenByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewAndIncludeChildrenByImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MATERIAL> GetViewByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetViewByImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MATERIAL> GetViewByImpMestIds(List<long> data)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_IMP_MEST_MATERIAL>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMaterialGet(param).GetViewByImpMestIds(Ids));
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
