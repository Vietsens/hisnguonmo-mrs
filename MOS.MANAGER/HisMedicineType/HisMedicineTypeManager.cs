using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineType
{
    public partial class HisMedicineTypeManager : BusinessBase
    {
        public HisMedicineTypeManager()
            : base()
        {

        }

        public HisMedicineTypeManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_MEDICINE_TYPE> Get(HisMedicineTypeFilterQuery filter)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).Get(filter);
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

        public HIS_MEDICINE_TYPE GetById(long data)
        {
            HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetById(data);
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

        public HIS_MEDICINE_TYPE GetById(long data, HisMedicineTypeFilterQuery filter)
        {
            HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetById(data, filter);
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

        public HIS_MEDICINE_TYPE GetByCode(string data)
        {
            HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByCode(data);
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

        public HIS_MEDICINE_TYPE GetByCode(string data, HisMedicineTypeFilterQuery filter)
        {
            HIS_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByCode(data, filter);
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

        public List<HIS_MEDICINE_TYPE> GetByIds(List<long> data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_MEDICINE_TYPE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisMedicineTypeGet(param).GetByIds(Ids));
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

        public List<HIS_MEDICINE_TYPE> GetByParentId(long data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByParentId(data);
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

        public List<HIS_MEDICINE_TYPE> GetByManufacturerId(long data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByManufacturerId(data);
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

        public List<HIS_MEDICINE_TYPE> GetByMedicineUseFormId(long data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByMedicineUseFormId(data);
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

        public List<HIS_MEDICINE_TYPE> GetByMedicineLineId(long data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByMedicineLineId(data);
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

        public List<HIS_MEDICINE_TYPE> GetByMemaGroupId(long data)
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetByMemaGroupId(data);
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

        public List<HIS_MEDICINE_TYPE> GetActiveAddictive()
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetActiveAddictive();
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

        public List<HIS_MEDICINE_TYPE> GetActiveNeuroLogical()
        {
            List<HIS_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeGet(param).GetActiveNeuroLogical();
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
