using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMedicine
{
    public partial class HisExpMestMedicineManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_MEDICINE> GetView(HisExpMestMedicineViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_MEDICINE GetViewById(long data)
        {
            V_HIS_EXP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_MEDICINE GetViewById(long data, HisExpMestMedicineViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewByTreatmentId(long data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewByTreatmentId(data);
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetViewByExpMestIds(Ids));
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewByIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetViewByIds(Ids));
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewRequestByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewRequestByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_MEDICINE> GetViewByAggrExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetViewByAggrExpMestId(data);
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
