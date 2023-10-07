using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceRetyCat
{
    public partial class HisServiceRetyCatManager : BusinessBase
    {
        public HisServiceRetyCatManager()
            : base()
        {

        }

        public HisServiceRetyCatManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_RETY_CAT> Get(HisServiceRetyCatFilterQuery filter)
        {
             List<HIS_SERVICE_RETY_CAT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_RETY_CAT> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRetyCatGet(param).Get(filter);
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

        
        public  HIS_SERVICE_RETY_CAT GetById(long data)
        {
             HIS_SERVICE_RETY_CAT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_RETY_CAT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRetyCatGet(param).GetById(data);
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

        
        public  HIS_SERVICE_RETY_CAT GetById(long data, HisServiceRetyCatFilterQuery filter)
        {
             HIS_SERVICE_RETY_CAT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_RETY_CAT resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRetyCatGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERVICE_RETY_CAT> GetByReportTypeCatId(long data)
        {
             List<HIS_SERVICE_RETY_CAT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERVICE_RETY_CAT> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceRetyCatGet(param).GetByReportTypeCatId(data);
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
