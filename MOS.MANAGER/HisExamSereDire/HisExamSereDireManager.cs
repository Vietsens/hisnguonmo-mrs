using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamSereDire
{
    public partial class HisExamSereDireManager : BusinessBase
    {
        public HisExamSereDireManager()
            : base()
        {

        }

        public HisExamSereDireManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_EXAM_SERE_DIRE> Get(HisExamSereDireFilterQuery filter)
        {
            List<HIS_EXAM_SERE_DIRE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXAM_SERE_DIRE> resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).Get(filter);
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

        
        public HIS_EXAM_SERE_DIRE GetById(long data)
        {
            HIS_EXAM_SERE_DIRE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EXAM_SERE_DIRE resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetById(data);
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

        
        public HIS_EXAM_SERE_DIRE GetById(long data, HisExamSereDireFilterQuery filter)
        {
            HIS_EXAM_SERE_DIRE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EXAM_SERE_DIRE resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetById(data, filter);
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

        
        public List<HIS_EXAM_SERE_DIRE> GetByDiseaseRelationId(long filter)
        {
            List<HIS_EXAM_SERE_DIRE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EXAM_SERE_DIRE> resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetByDiseaseRelationId(filter);
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
