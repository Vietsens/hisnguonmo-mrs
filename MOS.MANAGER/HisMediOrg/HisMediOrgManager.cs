using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediOrg
{
    public partial class HisMediOrgManager : BusinessBase
    {
        public HisMediOrgManager()
            : base()
        {

        }

        public HisMediOrgManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDI_ORG> Get(HisMediOrgFilterQuery filter)
        {
            List<HIS_MEDI_ORG> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_ORG> resultData = null;
                if (valid)
                {
                    resultData = new HisMediOrgGet(param).Get(filter);
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

        
        public HIS_MEDI_ORG GetById(long data)
        {
            HIS_MEDI_ORG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_ORG resultData = null;
                if (valid)
                {
                    resultData = new HisMediOrgGet(param).GetById(data);
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

        
        public HIS_MEDI_ORG GetById(long data, HisMediOrgFilterQuery filter)
        {
            HIS_MEDI_ORG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_ORG resultData = null;
                if (valid)
                {
                    resultData = new HisMediOrgGet(param).GetById(data, filter);
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

        
        public HIS_MEDI_ORG GetByCode(string data)
        {
            HIS_MEDI_ORG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_ORG resultData = null;
                if (valid)
                {
                    resultData = new HisMediOrgGet(param).GetByCode(data);
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

        
        public HIS_MEDI_ORG GetByCode(string data, HisMediOrgFilterQuery filter)
        {
            HIS_MEDI_ORG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_ORG resultData = null;
                if (valid)
                {
                    resultData = new HisMediOrgGet(param).GetByCode(data, filter);
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
