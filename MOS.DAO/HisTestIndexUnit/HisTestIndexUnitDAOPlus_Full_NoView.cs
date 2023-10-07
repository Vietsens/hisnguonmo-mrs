using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTestIndexUnit
{
    public partial class HisTestIndexUnitDAO : EntityBase
    {
        public HIS_TEST_INDEX_UNIT GetByCode(string code, HisTestIndexUnitSO search)
        {
            HIS_TEST_INDEX_UNIT result = null;

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

        public Dictionary<string, HIS_TEST_INDEX_UNIT> GetDicByCode(HisTestIndexUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_TEST_INDEX_UNIT> result = new Dictionary<string, HIS_TEST_INDEX_UNIT>();
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
