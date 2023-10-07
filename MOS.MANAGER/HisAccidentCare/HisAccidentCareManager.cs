using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentCare
{
    public partial class HisAccidentCareManager : BusinessBase
    {
        public HisAccidentCareManager()
            : base()
        {

        }

        public HisAccidentCareManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_CARE> Get(HisAccidentCareFilterQuery filter)
        {
             List<HIS_ACCIDENT_CARE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_CARE> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentCareGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_CARE GetById(long data)
        {
             HIS_ACCIDENT_CARE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_CARE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentCareGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_CARE GetById(long data, HisAccidentCareFilterQuery filter)
        {
             HIS_ACCIDENT_CARE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_CARE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentCareGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_CARE GetByCode(string data)
        {
             HIS_ACCIDENT_CARE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_CARE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentCareGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_CARE GetByCode(string data, HisAccidentCareFilterQuery filter)
        {
             HIS_ACCIDENT_CARE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_CARE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentCareGet(param).GetByCode(data, filter);
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
