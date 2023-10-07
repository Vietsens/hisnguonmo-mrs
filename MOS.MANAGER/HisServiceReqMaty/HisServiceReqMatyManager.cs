using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReqMaty
{
    public partial class HisServiceReqMatyManager : BusinessBase
    {
        public HisServiceReqMatyManager()
            : base()
        {

        }

        public HisServiceReqMatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_REQ_MATY> Get(HisServiceReqMatyFilterQuery filter)
        {
             List<HIS_SERVICE_REQ_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_REQ_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMatyGet(param).Get(filter);
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

        
        public  HIS_SERVICE_REQ_MATY GetById(long data)
        {
             HIS_SERVICE_REQ_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_REQ_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMatyGet(param).GetById(data);
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

        
        public  HIS_SERVICE_REQ_MATY GetById(long data, HisServiceReqMatyFilterQuery filter)
        {
             HIS_SERVICE_REQ_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_REQ_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqMatyGet(param).GetById(data, filter);
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
