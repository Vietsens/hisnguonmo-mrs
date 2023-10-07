using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceGroup
{
    public partial class HisServiceGroupManager : BusinessBase
    {
        public HisServiceGroupManager()
            : base()
        {

        }

        public HisServiceGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERVICE_GROUP> Get(HisServiceGroupFilterQuery filter)
        {
             List<HIS_SERVICE_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERVICE_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGroupGet(param).Get(filter);
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

        
        public  HIS_SERVICE_GROUP GetById(long data)
        {
             HIS_SERVICE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERVICE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGroupGet(param).GetById(data);
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

        
        public  HIS_SERVICE_GROUP GetById(long data, HisServiceGroupFilterQuery filter)
        {
             HIS_SERVICE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERVICE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisServiceGroupGet(param).GetById(data, filter);
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
