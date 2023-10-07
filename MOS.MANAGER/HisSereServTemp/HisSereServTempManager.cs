using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTemp
{
    public partial class HisSereServTempManager : BusinessBase
    {
        public HisSereServTempManager()
            : base()
        {

        }

        public HisSereServTempManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SERE_SERV_TEMP> Get(HisSereServTempFilterQuery filter)
        {
             List<HIS_SERE_SERV_TEMP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SERE_SERV_TEMP> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTempGet(param).Get(filter);
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

        
        public  HIS_SERE_SERV_TEMP GetById(long data)
        {
             HIS_SERE_SERV_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SERE_SERV_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTempGet(param).GetById(data);
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

        
        public  HIS_SERE_SERV_TEMP GetById(long data, HisSereServTempFilterQuery filter)
        {
             HIS_SERE_SERV_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SERE_SERV_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisSereServTempGet(param).GetById(data, filter);
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
