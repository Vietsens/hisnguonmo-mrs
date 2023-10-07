using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestMedicine
{
    public partial class HisImpMestMedicineManager : BusinessBase
    {
        public HisImpMestMedicineManager()
            : base()
        {

        }

        public HisImpMestMedicineManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_IMP_MEST_MEDICINE> Get(HisImpMestMedicineFilterQuery filter)
        {
            List<HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).Get(filter);
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

        
        public HIS_IMP_MEST_MEDICINE GetById(long data)
        {
            HIS_IMP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_IMP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetById(data);
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

        
        public HIS_IMP_MEST_MEDICINE GetById(long data, HisImpMestMedicineFilterQuery filter)
        {
            HIS_IMP_MEST_MEDICINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_IMP_MEST_MEDICINE resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetById(data, filter);
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

        
        public List<HIS_IMP_MEST_MEDICINE> GetByMedicineId(long data)
        {
            List<HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetByMedicineId(data);
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

        
        public List<HIS_IMP_MEST_MEDICINE> GetByImpMestId(long data)
        {
            List<HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new HisImpMestMedicineGet(param).GetByImpMestId(data);
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

        
        public List<HIS_IMP_MEST_MEDICINE> GetByImpMestIds(List<long> data)
        {
            List<HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_IMP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMedicineGet(param).GetByImpMestIds(Ids));
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

        
        public List<HIS_IMP_MEST_MEDICINE> GetByIds(List<long> data)
        {
            List<HIS_IMP_MEST_MEDICINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_IMP_MEST_MEDICINE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_IMP_MEST_MEDICINE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisImpMestMedicineGet(param).GetByIds(Ids));
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
