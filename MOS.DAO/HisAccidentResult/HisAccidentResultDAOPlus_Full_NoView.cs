using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentResult
{
    public partial class HisAccidentResultDAO : EntityBase
    {
        public HIS_ACCIDENT_RESULT GetByCode(string code, HisAccidentResultSO search)
        {
            HIS_ACCIDENT_RESULT result = null;

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

        public Dictionary<string, HIS_ACCIDENT_RESULT> GetDicByCode(HisAccidentResultSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_RESULT> result = new Dictionary<string, HIS_ACCIDENT_RESULT>();
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
