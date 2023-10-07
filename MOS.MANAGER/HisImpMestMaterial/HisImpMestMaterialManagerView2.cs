using Inventec.Common.Logging;
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
        public List<V_HIS_IMP_MEST_MATERIAL_2> GetView2(HisImpMestMaterialView2FilterQuery filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL_2> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_MATERIAL_2> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView2(filter);
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

        public V_HIS_IMP_MEST_MATERIAL_2 GetView2ById(long data)
        {
            V_HIS_IMP_MEST_MATERIAL_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MATERIAL_2 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView2ById(data);
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

        public V_HIS_IMP_MEST_MATERIAL_2 GetView2ById(long data, HisImpMestMaterialView2FilterQuery filter)
        {
            V_HIS_IMP_MEST_MATERIAL_2 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MATERIAL_2 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView2ById(data, filter);
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

        public List<V_HIS_IMP_MEST_MATERIAL_2> GetView2ByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL_2> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL_2> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView2ByImpMestId(data);
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

        public List<V_HIS_IMP_MEST_MATERIAL_2> GetView2ByImpMestIds(List<long> data)
        {
            List<V_HIS_IMP_MEST_MATERIAL_2> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL_2> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_IMP_MEST_MATERIAL_2>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMaterialGet(param).GetView2ByImpMestIds(Ids));
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
