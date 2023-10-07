using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMety
{
    public partial class HisServiceMetyManager : BusinessBase
    {
        public HisServiceMetyManager()
            : base()
        {

        }

        public HisServiceMetyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_METY> Get(HisServiceMetyFilterQuery filter)
        {
             List<HIS_SERVICE_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).Get(filter);
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

        
        public  HIS_SERVICE_METY GetById(long data)
        {
             HIS_SERVICE_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_METY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).GetById(data);
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

        
        public  HIS_SERVICE_METY GetById(long data, HisServiceMetyFilterQuery filter)
        {
             HIS_SERVICE_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_METY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERVICE_METY> GetByMaterialTypeId(long data)
        {
             List<HIS_SERVICE_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).GetByMaterialTypeId(data);
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

        
        public  List<HIS_SERVICE_METY> GetByServiceId(long data)
        {
             List<HIS_SERVICE_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMetyGet(param).GetByServiceId(data);
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
