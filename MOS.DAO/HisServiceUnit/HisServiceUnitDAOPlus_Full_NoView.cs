using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceUnit
{
    public partial class HisServiceUnitDAO : EntityBase
    {
        public HIS_SERVICE_UNIT GetByCode(string code, HisServiceUnitSO search)
        {
            HIS_SERVICE_UNIT result = null;

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

        public Dictionary<string, HIS_SERVICE_UNIT> GetDicByCode(HisServiceUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_UNIT> result = new Dictionary<string, HIS_SERVICE_UNIT>();
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
