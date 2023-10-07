using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisManufacturer
{
    public partial class HisManufacturerManager : BusinessBase
    {
        public HisManufacturerManager()
            : base()
        {

        }

        public HisManufacturerManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MANUFACTURER> Get(HisManufacturerFilterQuery filter)
        {
             List<HIS_MANUFACTURER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MANUFACTURER> resultData = null;
                if (valid)
                {
                    resultData = new HisManufacturerGet(param).Get(filter);
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

        
        public  HIS_MANUFACTURER GetById(long data)
        {
             HIS_MANUFACTURER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MANUFACTURER resultData = null;
                if (valid)
                {
                    resultData = new HisManufacturerGet(param).GetById(data);
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

        
        public  HIS_MANUFACTURER GetById(long data, HisManufacturerFilterQuery filter)
        {
             HIS_MANUFACTURER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MANUFACTURER resultData = null;
                if (valid)
                {
                    resultData = new HisManufacturerGet(param).GetById(data, filter);
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

        
        public  HIS_MANUFACTURER GetByCode(string data)
        {
             HIS_MANUFACTURER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MANUFACTURER resultData = null;
                if (valid)
                {
                    resultData = new HisManufacturerGet(param).GetByCode(data);
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

        
        public  HIS_MANUFACTURER GetByCode(string data, HisManufacturerFilterQuery filter)
        {
             HIS_MANUFACTURER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MANUFACTURER resultData = null;
                if (valid)
                {
                    resultData = new HisManufacturerGet(param).GetByCode(data, filter);
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
