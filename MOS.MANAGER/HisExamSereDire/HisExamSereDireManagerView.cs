using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamSereDire
{
    public partial class HisExamSereDireManager : BusinessBase
    {
        
        public List<V_HIS_EXAM_SERE_DIRE> GetView(HisExamSereDireViewFilterQuery filter)
        {
            List<V_HIS_EXAM_SERE_DIRE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXAM_SERE_DIRE> resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetView(filter);
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

        
        public V_HIS_EXAM_SERE_DIRE GetViewById(long data)
        {
            V_HIS_EXAM_SERE_DIRE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXAM_SERE_DIRE resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetViewById(data);
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

        
        public V_HIS_EXAM_SERE_DIRE GetViewById(long data, HisExamSereDireViewFilterQuery filter)
        {
            V_HIS_EXAM_SERE_DIRE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXAM_SERE_DIRE resultData = null;
                if (valid)
                {
                    resultData = new HisExamSereDireGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_EXAM_SERE_DIRE> GetViewByIds(List<long> filter)
        {
            List<V_HIS_EXAM_SERE_DIRE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXAM_SERE_DIRE> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_EXAM_SERE_DIRE>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData = new HisExamSereDireGet(param).GetViewByIds(Ids);
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
    }
}
