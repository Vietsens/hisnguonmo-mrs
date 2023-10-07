using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRemuneration
{
    public partial class HisRemunerationManager : BusinessBase
    {
        public HisRemunerationManager()
            : base()
        {

        }

        public HisRemunerationManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REMUNERATION> Get(HisRemunerationFilterQuery filter)
        {
             List<HIS_REMUNERATION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REMUNERATION> resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).Get(filter);
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

        
        public  HIS_REMUNERATION GetById(long data)
        {
             HIS_REMUNERATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REMUNERATION resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).GetById(data);
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

        
        public  HIS_REMUNERATION GetById(long data, HisRemunerationFilterQuery filter)
        {
             HIS_REMUNERATION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REMUNERATION resultData = null;
                if (valid)
                {
                    resultData = new HisRemunerationGet(param).GetById(data, filter);
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
