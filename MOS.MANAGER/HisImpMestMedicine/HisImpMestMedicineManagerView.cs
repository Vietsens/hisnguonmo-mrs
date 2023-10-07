using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public partial class HisImpMestMedicineManager : BusinessBase
    {
        
        public List<V_HIS_IMP_MEST_MEDICINE> GetView(HisImpMestMedicineViewFilterQuery filter)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetView(filter);
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

        
        public V_HIS_IMP_MEST_MEDICINE GetViewById(long data)
        {
            V_HIS_IMP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_IMP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewById(data);
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

        
        public V_HIS_IMP_MEST_MEDICINE GetViewById(long data, HisImpMestMedicineViewFilterQuery filter)
        {
            V_HIS_IMP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_IMP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_IMP_MEST_MEDICINE> GetViewAndIncludeChildrenByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewAndIncludeChildrenByImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MEDICINE> GetViewByAggrImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewByAggrImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MEDICINE> GetViewByAggrImpMestIdAndGroupByMedicine(long data)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewByAggrImpMestIdAndGroupByMedicine(data);
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

        
        public List<V_HIS_IMP_MEST_MEDICINE> GetViewByImpMestId(long data)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetViewByImpMestId(data);
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

        
        public List<V_HIS_IMP_MEST_MEDICINE> GetViewByImpMestIds(List<long> data)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_IMP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMedicineGet(param).GetViewByImpMestIds(Ids));
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
