using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodAbo
{
    public partial class HisBloodAboManager : BusinessBase
    {
        public HisBloodAboManager()
            : base()
        {

        }

        public HisBloodAboManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BLOOD_ABO> Get(HisBloodAboFilterQuery filter)
        {
             List<HIS_BLOOD_ABO> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BLOOD_ABO> resultData = null;
                if (valid)
                {
                    resultData = new HisBloodAboGet(param).Get(filter);
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

        
        public  HIS_BLOOD_ABO GetById(long data)
        {
             HIS_BLOOD_ABO result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_ABO resultData = null;
                if (valid)
                {
                    resultData = new HisBloodAboGet(param).GetById(data);
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

        
        public  HIS_BLOOD_ABO GetById(long data, HisBloodAboFilterQuery filter)
        {
             HIS_BLOOD_ABO result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_ABO resultData = null;
                if (valid)
                {
                    resultData = new HisBloodAboGet(param).GetById(data, filter);
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

        
        public  HIS_BLOOD_ABO GetByCode(string data)
        {
             HIS_BLOOD_ABO result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BLOOD_ABO resultData = null;
                if (valid)
                {
                    resultData = new HisBloodAboGet(param).GetByCode(data);
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

        
        public  HIS_BLOOD_ABO GetByCode(string data, HisBloodAboFilterQuery filter)
        {
             HIS_BLOOD_ABO result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BLOOD_ABO resultData = null;
                if (valid)
                {
                    resultData = new HisBloodAboGet(param).GetByCode(data, filter);
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
