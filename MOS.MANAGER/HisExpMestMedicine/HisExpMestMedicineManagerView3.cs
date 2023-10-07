using Inventec.Common.Logging;
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
        public List<V_HIS_EXP_MEST_MEDICINE_3> GetView3(HisExpMestMedicineView3FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MEDICINE_3> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_MEDICINE_3> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetView3(filter);
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

        public V_HIS_EXP_MEST_MEDICINE_3 GetView3ById(long data)
        {
            V_HIS_EXP_MEST_MEDICINE_3 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_MEDICINE_3 resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetView3ById(data);
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

        public V_HIS_EXP_MEST_MEDICINE_3 GetView3ById(long data, HisExpMestMedicineView3FilterQuery filter)
        {
            V_HIS_EXP_MEST_MEDICINE_3 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_MEDICINE_3 resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetView3ById(data, filter);
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

        public List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MEDICINE_3> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE_3> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MEDICINE_3>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetView3ByIds(Ids));
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

        public List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_MEDICINE_3> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE_3> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMedicineGet(param).GetView3ByExpMestId(data);
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

        public List<V_HIS_EXP_MEST_MEDICINE_3> GetView3ByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_MEDICINE_3> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_MEDICINE_3> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXP_MEST_MEDICINE_3>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisExpMestMedicineGet(param).GetView3ByExpMestIds(data));
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
