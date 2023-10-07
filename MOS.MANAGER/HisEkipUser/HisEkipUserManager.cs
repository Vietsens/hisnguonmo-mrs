using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipUser
{
    public partial class HisEkipUserManager : BusinessBase
    {
        public HisEkipUserManager()
            : base()
        {

        }

        public HisEkipUserManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EKIP_USER> Get(HisEkipUserFilterQuery filter)
        {
             List<HIS_EKIP_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EKIP_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisEkipUserGet(param).Get(filter);
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

        
        public  HIS_EKIP_USER GetById(long data)
        {
             HIS_EKIP_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EKIP_USER resultData = null;
                if (valid)
                {
                    resultData = new HisEkipUserGet(param).GetById(data);
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

        
        public  HIS_EKIP_USER GetById(long data, HisEkipUserFilterQuery filter)
        {
             HIS_EKIP_USER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EKIP_USER resultData = null;
                if (valid)
                {
                    resultData = new HisEkipUserGet(param).GetById(data, filter);
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

        
        public  List<HIS_EKIP_USER> GetByEkipId(long filter)
        {
             List<HIS_EKIP_USER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EKIP_USER> resultData = null;
                if (valid)
                {
                    resultData = new HisEkipUserGet(param).GetByEkipId(filter);
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
