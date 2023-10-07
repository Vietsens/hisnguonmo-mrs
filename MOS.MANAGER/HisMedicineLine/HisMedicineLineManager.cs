using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineLine
{
    public partial class HisMedicineLineManager : BusinessBase
    {
        public HisMedicineLineManager()
            : base()
        {

        }

        public HisMedicineLineManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDICINE_LINE> Get(HisMedicineLineFilterQuery filter)
        {
             List<HIS_MEDICINE_LINE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_LINE> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineLineGet(param).Get(filter);
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

        
        public  HIS_MEDICINE_LINE GetById(long data)
        {
             HIS_MEDICINE_LINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_LINE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineLineGet(param).GetById(data);
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

        
        public  HIS_MEDICINE_LINE GetById(long data, HisMedicineLineFilterQuery filter)
        {
             HIS_MEDICINE_LINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_LINE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineLineGet(param).GetById(data, filter);
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

        
        public  HIS_MEDICINE_LINE GetByCode(string data)
        {
             HIS_MEDICINE_LINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_LINE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineLineGet(param).GetByCode(data);
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

        
        public  HIS_MEDICINE_LINE GetByCode(string data, HisMedicineLineFilterQuery filter)
        {
             HIS_MEDICINE_LINE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_LINE resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineLineGet(param).GetByCode(data, filter);
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
