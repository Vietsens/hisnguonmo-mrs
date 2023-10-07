using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineUseForm
{
    public partial class HisMedicineUseFormManager : BusinessBase
    {
        public HisMedicineUseFormManager()
            : base()
        {

        }

        public HisMedicineUseFormManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDICINE_USE_FORM> Get(HisMedicineUseFormFilterQuery filter)
        {
             List<HIS_MEDICINE_USE_FORM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_USE_FORM> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineUseFormGet(param).Get(filter);
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

        
        public  HIS_MEDICINE_USE_FORM GetById(long data)
        {
             HIS_MEDICINE_USE_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_USE_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineUseFormGet(param).GetById(data);
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

        
        public  HIS_MEDICINE_USE_FORM GetById(long data, HisMedicineUseFormFilterQuery filter)
        {
             HIS_MEDICINE_USE_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_USE_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineUseFormGet(param).GetById(data, filter);
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

        
        public  HIS_MEDICINE_USE_FORM GetByCode(string data)
        {
             HIS_MEDICINE_USE_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_USE_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineUseFormGet(param).GetByCode(data);
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

        
        public  HIS_MEDICINE_USE_FORM GetByCode(string data, HisMedicineUseFormFilterQuery filter)
        {
             HIS_MEDICINE_USE_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_USE_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineUseFormGet(param).GetByCode(data, filter);
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
