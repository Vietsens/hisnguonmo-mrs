using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAcinInteractive
{
    public partial class HisAcinInteractiveManager : BusinessBase
    {
        public HisAcinInteractiveManager()
            : base()
        {

        }

        public HisAcinInteractiveManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACIN_INTERACTIVE> Get(HisAcinInteractiveFilterQuery filter)
        {
             List<HIS_ACIN_INTERACTIVE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACIN_INTERACTIVE> resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).Get(filter);
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

        
        public  HIS_ACIN_INTERACTIVE GetById(long data)
        {
             HIS_ACIN_INTERACTIVE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACIN_INTERACTIVE resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).GetById(data);
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

        
        public  HIS_ACIN_INTERACTIVE GetById(long data, HisAcinInteractiveFilterQuery filter)
        {
             HIS_ACIN_INTERACTIVE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACIN_INTERACTIVE resultData = null;
                if (valid)
                {
                    resultData = new HisAcinInteractiveGet(param).GetById(data, filter);
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
