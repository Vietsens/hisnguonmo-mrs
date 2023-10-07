using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTranPatiForm
{
    public partial class HisTranPatiFormManager : BusinessBase
    {
        public HisTranPatiFormManager()
            : base()
        {

        }

        public HisTranPatiFormManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TRAN_PATI_FORM> Get(HisTranPatiFormFilterQuery filter)
        {
             List<HIS_TRAN_PATI_FORM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRAN_PATI_FORM> resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiFormGet(param).Get(filter);
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

        
        public  HIS_TRAN_PATI_FORM GetById(long data)
        {
             HIS_TRAN_PATI_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRAN_PATI_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiFormGet(param).GetById(data);
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

        
        public  HIS_TRAN_PATI_FORM GetById(long data, HisTranPatiFormFilterQuery filter)
        {
             HIS_TRAN_PATI_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRAN_PATI_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiFormGet(param).GetById(data, filter);
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

        
        public  HIS_TRAN_PATI_FORM GetByCode(string data)
        {
             HIS_TRAN_PATI_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRAN_PATI_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiFormGet(param).GetByCode(data);
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

        
        public  HIS_TRAN_PATI_FORM GetByCode(string data, HisTranPatiFormFilterQuery filter)
        {
             HIS_TRAN_PATI_FORM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRAN_PATI_FORM resultData = null;
                if (valid)
                {
                    resultData = new HisTranPatiFormGet(param).GetByCode(data, filter);
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
