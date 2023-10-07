using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceMaty
{
    public partial class HisServiceMatyManager : BusinessBase
    {
        public HisServiceMatyManager()
            : base()
        {

        }

        public HisServiceMatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_MATY> Get(HisServiceMatyFilterQuery filter)
        {
             List<HIS_SERVICE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).Get(filter);
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

        
        public  HIS_SERVICE_MATY GetById(long data)
        {
             HIS_SERVICE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).GetById(data);
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

        
        public  HIS_SERVICE_MATY GetById(long data, HisServiceMatyFilterQuery filter)
        {
             HIS_SERVICE_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERVICE_MATY> GetByMaterialTypeId(long data)
        {
             List<HIS_SERVICE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).GetByMaterialTypeId(data);
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

        
        public  List<HIS_SERVICE_MATY> GetByServiceId(long data)
        {
             List<HIS_SERVICE_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceMatyGet(param).GetByServiceId(data);
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
