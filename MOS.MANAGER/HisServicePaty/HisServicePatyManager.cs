using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePaty
{
    public partial class HisServicePatyManager : BusinessBase
    {
        public HisServicePatyManager()
            : base()
        {

        }

        public HisServicePatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_PATY> Get(HisServicePatyFilterQuery filter)
        {
             List<HIS_SERVICE_PATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_PATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).Get(filter);
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

        
        public  HIS_SERVICE_PATY GetById(long data)
        {
             HIS_SERVICE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetById(data);
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

        
        public  HIS_SERVICE_PATY GetById(long data, HisServicePatyFilterQuery filter)
        {
             HIS_SERVICE_PATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_PATY resultData = null;
                if (valid)
                {
                    resultData = new HisServicePatyGet(param).GetById(data, filter);
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
