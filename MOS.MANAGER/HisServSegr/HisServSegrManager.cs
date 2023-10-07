using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServSegr
{
    public partial class HisServSegrManager : BusinessBase
    {
        public HisServSegrManager()
            : base()
        {

        }

        public HisServSegrManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERV_SEGR> Get(HisServSegrFilterQuery filter)
        {
             List<HIS_SERV_SEGR> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERV_SEGR> resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).Get(filter);
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

        
        public  HIS_SERV_SEGR GetById(long data)
        {
             HIS_SERV_SEGR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERV_SEGR resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetById(data);
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

        
        public  HIS_SERV_SEGR GetById(long data, HisServSegrFilterQuery filter)
        {
             HIS_SERV_SEGR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERV_SEGR resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetById(data, filter);
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

        
        public  List<HIS_SERV_SEGR> GetByServiceId(long data)
        {
             List<HIS_SERV_SEGR> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERV_SEGR> resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetByServiceId(data);
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

        
        public  List<HIS_SERV_SEGR> GetByServiceGroupId(long data)
        {
             List<HIS_SERV_SEGR> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SERV_SEGR> resultData = null;
                if (valid)
                {
                    resultData = new HisServSegrGet(param).GetByServiceGroupId(data);
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
