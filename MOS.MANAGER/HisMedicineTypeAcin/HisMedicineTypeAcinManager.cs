using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeAcin
{
    public partial class HisMedicineTypeAcinManager : BusinessBase
    {
        public HisMedicineTypeAcinManager()
            : base()
        {

        }

        public HisMedicineTypeAcinManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDICINE_TYPE_ACIN> Get(HisMedicineTypeAcinFilterQuery filter)
        {
             List<HIS_MEDICINE_TYPE_ACIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_TYPE_ACIN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).Get(filter);
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

        
        public  HIS_MEDICINE_TYPE_ACIN GetById(long data)
        {
             HIS_MEDICINE_TYPE_ACIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_TYPE_ACIN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetById(data);
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

        
        public  HIS_MEDICINE_TYPE_ACIN GetById(long data, HisMedicineTypeAcinFilterQuery filter)
        {
             HIS_MEDICINE_TYPE_ACIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_TYPE_ACIN resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEDICINE_TYPE_ACIN> GetByActiveIngredientId(long data)
        {
             List<HIS_MEDICINE_TYPE_ACIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDICINE_TYPE_ACIN> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeAcinGet(param).GetByActiveIngredientId(data);
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
