using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatySub
{
    public partial class HisMestPatySubManager : BusinessBase
    {
        public HisMestPatySubManager()
            : base()
        {

        }

        public HisMestPatySubManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PATY_SUB> Get(HisMestPatySubFilterQuery filter)
        {
             List<HIS_MEST_PATY_SUB> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PATY_SUB> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).Get(filter);
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

        
        public  HIS_MEST_PATY_SUB GetById(long data)
        {
             HIS_MEST_PATY_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PATY_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).GetById(data);
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

        
        public  HIS_MEST_PATY_SUB GetById(long data, HisMestPatySubFilterQuery filter)
        {
             HIS_MEST_PATY_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PATY_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatySubGet(param).GetById(data, filter);
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
