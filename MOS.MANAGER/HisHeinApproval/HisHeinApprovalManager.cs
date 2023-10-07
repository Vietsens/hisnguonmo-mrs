using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinApproval
{
    public partial class HisHeinApprovalManager : BusinessBase
    {
        public HisHeinApprovalManager()
            : base()
        {

        }

        public HisHeinApprovalManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_HEIN_APPROVAL> Get(HisHeinApprovalFilterQuery filter)
        {
            List<HIS_HEIN_APPROVAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_HEIN_APPROVAL> resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).Get(filter);
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

        
        public HIS_HEIN_APPROVAL GetById(long data)
        {
            HIS_HEIN_APPROVAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_HEIN_APPROVAL resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetById(data);
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

        
        public HIS_HEIN_APPROVAL GetById(long data, HisHeinApprovalFilterQuery filter)
        {
            HIS_HEIN_APPROVAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_HEIN_APPROVAL resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetById(data, filter);
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

        
        public List<HIS_HEIN_APPROVAL> GetByTreatmentId(long data)
        {
            List<HIS_HEIN_APPROVAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_HEIN_APPROVAL> resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetByTreatmentId(data);
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

        
        public List<HIS_HEIN_APPROVAL> GetByTreatmentIds(List<long> data)
        {
            List<HIS_HEIN_APPROVAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_HEIN_APPROVAL> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_HEIN_APPROVAL>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisHeinApprovalGet(param).GetByTreatmentIds(Ids));
                    }
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

        
        public List<HIS_HEIN_APPROVAL> GetExistedData(HIS_HEIN_APPROVAL data, long treatmentId)
        {
            List<HIS_HEIN_APPROVAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(treatmentId);
                List<HIS_HEIN_APPROVAL> resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetExistedData(data, treatmentId);
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
