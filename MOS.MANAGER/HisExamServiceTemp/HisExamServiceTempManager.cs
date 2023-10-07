using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamServiceTemp
{
    public partial class HisExamServiceTempManager : BusinessBase
    {
        public HisExamServiceTempManager()
            : base()
        {

        }

        public HisExamServiceTempManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EXAM_SERVICE_TEMP> Get(HisExamServiceTempFilterQuery filter)
        {
             List<HIS_EXAM_SERVICE_TEMP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXAM_SERVICE_TEMP> resultData = null;
                if (valid)
                {
                    resultData = new HisExamServiceTempGet(param).Get(filter);
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

        
        public  HIS_EXAM_SERVICE_TEMP GetById(long data)
        {
             HIS_EXAM_SERVICE_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXAM_SERVICE_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisExamServiceTempGet(param).GetById(data);
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

        
        public  HIS_EXAM_SERVICE_TEMP GetById(long data, HisExamServiceTempFilterQuery filter)
        {
             HIS_EXAM_SERVICE_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXAM_SERVICE_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisExamServiceTempGet(param).GetById(data, filter);
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

        
        public  HIS_EXAM_SERVICE_TEMP GetByCode(string data)
        {
             HIS_EXAM_SERVICE_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXAM_SERVICE_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisExamServiceTempGet(param).GetByCode(data);
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

        
        public  HIS_EXAM_SERVICE_TEMP GetByCode(string data, HisExamServiceTempFilterQuery filter)
        {
             HIS_EXAM_SERVICE_TEMP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXAM_SERVICE_TEMP resultData = null;
                if (valid)
                {
                    resultData = new HisExamServiceTempGet(param).GetByCode(data, filter);
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
