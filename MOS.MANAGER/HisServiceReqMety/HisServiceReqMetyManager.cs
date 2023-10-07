using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqMety
{
    public partial class HisServiceReqMetyManager : BusinessBase
    {
        public HisServiceReqMetyManager()
            : base()
        {

        }

        public HisServiceReqMetyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_REQ_METY> Get(HisServiceReqMetyFilterQuery filter)
        {
             List<HIS_SERVICE_REQ_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_REQ_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMetyGet(param).Get(filter);
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

        
        public  HIS_SERVICE_REQ_METY GetById(long data)
        {
             HIS_SERVICE_REQ_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_METY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMetyGet(param).GetById(data);
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

        
        public  HIS_SERVICE_REQ_METY GetById(long data, HisServiceReqMetyFilterQuery filter)
        {
             HIS_SERVICE_REQ_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_METY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMetyGet(param).GetById(data, filter);
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
