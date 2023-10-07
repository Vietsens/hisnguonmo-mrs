using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicine
{
    public partial class HisMedicineManager : BusinessBase
    {
        public List<V_HIS_MEDICINE_1> GetView1(HisMedicineView1FilterQuery filter)
        {
            List<V_HIS_MEDICINE_1> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDICINE_1> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGet(param).GetView1(filter);
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

        public V_HIS_MEDICINE_1 GetView1ById(long data)
        {
            V_HIS_MEDICINE_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDICINE_1 resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGet(param).GetView1ById(data);
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

        public V_HIS_MEDICINE_1 GetView1ById(long data, HisMedicineView1FilterQuery filter)
        {
            V_HIS_MEDICINE_1 result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDICINE_1 resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGet(param).GetView1ById(data, filter);
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

        public List<V_HIS_MEDICINE_1> GetView1ByIds(List<long> data)
        {
            List<V_HIS_MEDICINE_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_MEDICINE_1> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_MEDICINE_1>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineGet(param).GetView1ByIds(Ids));
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
