using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTestIndexRange
{
    public partial class HisTestIndexRangeDAO : EntityBase
    {
        private HisTestIndexRangeGet GetWorker
        {
            get
            {
                return (HisTestIndexRangeGet)Worker.Get<HisTestIndexRangeGet>();
            }
        }
        public List<HIS_TEST_INDEX_RANGE> Get(HisTestIndexRangeSO search, CommonParam param)
        {
            List<HIS_TEST_INDEX_RANGE> result = new List<HIS_TEST_INDEX_RANGE>();
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

        public HIS_TEST_INDEX_RANGE GetById(long id, HisTestIndexRangeSO search)
        {
            HIS_TEST_INDEX_RANGE result = null;
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
