using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    public partial class HisMedicineBeanManager : BusinessBase
    {
        public HisMedicineBeanManager()
            : base()
        {

        }

        public HisMedicineBeanManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDICINE_BEAN> Get(HisMedicineBeanFilterQuery filter)
        {
            List<HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).Get(filter);
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

        
        public HIS_MEDICINE_BEAN GetById(long data)
        {
            HIS_MEDICINE_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetById(data);
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

        
        public HIS_MEDICINE_BEAN GetById(long data, HisMedicineBeanFilterQuery filter)
        {
            HIS_MEDICINE_BEAN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_BEAN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetById(data, filter);
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

        
        public List<HIS_MEDICINE_BEAN> GetByIds(List<long> data)
        {
            List<HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MEDICINE_BEAN>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineBeanGet(param).GetByIds(Ids));
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

        
        public List<HIS_MEDICINE_BEAN> GetByMediStockId(long data)
        {
            List<HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetByMediStockId(data);
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

        
        public List<HIS_MEDICINE_BEAN> GetByMedicineId(long data)
        {
            List<HIS_MEDICINE_BEAN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_BEAN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineBeanGet(param).GetByMedicineId(data);
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
