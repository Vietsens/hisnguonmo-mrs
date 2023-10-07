using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTranPatiReason
{
    public partial class HisTranPatiReasonManager : BusinessBase
    {
        public HisTranPatiReasonManager()
            : base()
        {

        }

        public HisTranPatiReasonManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TRAN_PATI_REASON> Get(HisTranPatiReasonFilterQuery filter)
        {
             List<HIS_TRAN_PATI_REASON> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRAN_PATI_REASON> resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiReasonGet(param).Get(filter);
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

        
        public  HIS_TRAN_PATI_REASON GetById(long data)
        {
             HIS_TRAN_PATI_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRAN_PATI_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiReasonGet(param).GetById(data);
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

        
        public  HIS_TRAN_PATI_REASON GetById(long data, HisTranPatiReasonFilterQuery filter)
        {
             HIS_TRAN_PATI_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRAN_PATI_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiReasonGet(param).GetById(data, filter);
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

        
        public  HIS_TRAN_PATI_REASON GetByCode(string data)
        {
             HIS_TRAN_PATI_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRAN_PATI_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiReasonGet(param).GetByCode(data);
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

        
        public  HIS_TRAN_PATI_REASON GetByCode(string data, HisTranPatiReasonFilterQuery filter)
        {
             HIS_TRAN_PATI_REASON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRAN_PATI_REASON resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiReasonGet(param).GetByCode(data, filter);
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
