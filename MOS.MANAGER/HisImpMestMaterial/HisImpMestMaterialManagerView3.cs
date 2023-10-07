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
        public List<V_HIS_IMP_MEST_MATERIAL_3> GetView3(HisImpMestMaterialView3FilterQuery filter)
        {
            List<V_HIS_IMP_MEST_MATERIAL_3> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_MATERIAL_3> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView3(filter);
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

        public V_HIS_IMP_MEST_MATERIAL_3 GetView3ById(long data)
        {
            V_HIS_IMP_MEST_MATERIAL_3 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MATERIAL_3 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView3ById(data);
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

        public V_HIS_IMP_MEST_MATERIAL_3 GetView3ById(long data, HisImpMestMaterialView3FilterQuery filter)
        {
            V_HIS_IMP_MEST_MATERIAL_3 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MATERIAL_3 resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView3ById(data, filter);
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

        public List<V_HIS_IMP_MEST_MATERIAL_3> GetView3ByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MATERIAL_3> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL_3> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMaterialGet(param).GetView3ByImpMestId(data);
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

        public List<V_HIS_IMP_MEST_MATERIAL_3> GetView3ByImpMestIds(List<long> data)
        {
            List<V_HIS_IMP_MEST_MATERIAL_3> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MATERIAL_3> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_IMP_MEST_MATERIAL_3>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMaterialGet(param).GetView3ByImpMestIds(Ids));
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
