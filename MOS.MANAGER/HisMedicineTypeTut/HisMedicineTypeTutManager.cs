using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeTut
{
    public partial class HisMedicineTypeTutManager : BusinessBase
    {
        public HisMedicineTypeTutManager()
            : base()
        {

        }

        public HisMedicineTypeTutManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDICINE_TYPE_TUT> Get(HisMedicineTypeTutFilterQuery filter)
        {
             List<HIS_MEDICINE_TYPE_TUT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_TYPE_TUT> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).Get(filter);
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

        
        public  HIS_MEDICINE_TYPE_TUT GetById(long data)
        {
             HIS_MEDICINE_TYPE_TUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_TYPE_TUT resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).GetById(data);
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

        
        public  HIS_MEDICINE_TYPE_TUT GetById(long data, HisMedicineTypeTutFilterQuery filter)
        {
             HIS_MEDICINE_TYPE_TUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_TYPE_TUT resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineTypeTutGet(param).GetById(data, filter);
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
