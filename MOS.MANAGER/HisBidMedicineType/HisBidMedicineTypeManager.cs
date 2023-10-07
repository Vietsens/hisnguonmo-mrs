using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMedicineType
{
    public partial class HisBidMedicineTypeManager : BusinessBase
    {
        public HisBidMedicineTypeManager()
            : base()
        {

        }

        public HisBidMedicineTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BID_MEDICINE_TYPE> Get(HisBidMedicineTypeFilterQuery filter)
        {
             List<HIS_BID_MEDICINE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BID_MEDICINE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).Get(filter);
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

        
        public  HIS_BID_MEDICINE_TYPE GetById(long data)
        {
             HIS_BID_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BID_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).GetById(data);
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

        
        public  HIS_BID_MEDICINE_TYPE GetById(long data, HisBidMedicineTypeFilterQuery filter)
        {
             HIS_BID_MEDICINE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BID_MEDICINE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMedicineTypeGet(param).GetById(data, filter);
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
