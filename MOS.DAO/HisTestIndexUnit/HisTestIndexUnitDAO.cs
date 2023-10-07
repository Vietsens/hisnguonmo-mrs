using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTestIndexUnit
{
    public partial class HisTestIndexUnitDAO : EntityBase
    {
        private HisTestIndexUnitGet GetWorker
        {
            get
            {
                return (HisTestIndexUnitGet)Worker.Get<HisTestIndexUnitGet>();
            }
        }
        public List<HIS_TEST_INDEX_UNIT> Get(HisTestIndexUnitSO search, CommonParam param)
        {
            List<HIS_TEST_INDEX_UNIT> result = new List<HIS_TEST_INDEX_UNIT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_TEST_INDEX_UNIT GetById(long id, HisTestIndexUnitSO search)
        {
            HIS_TEST_INDEX_UNIT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
