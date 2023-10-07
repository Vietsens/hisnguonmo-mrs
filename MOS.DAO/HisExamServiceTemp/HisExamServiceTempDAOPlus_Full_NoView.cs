using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExamServiceTemp
{
    public partial class HisExamServiceTempDAO : EntityBase
    {
        public HIS_EXAM_SERVICE_TEMP GetByCode(string code, HisExamServiceTempSO search)
        {
            HIS_EXAM_SERVICE_TEMP result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_EXAM_SERVICE_TEMP> GetDicByCode(HisExamServiceTempSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXAM_SERVICE_TEMP> result = new Dictionary<string, HIS_EXAM_SERVICE_TEMP>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
