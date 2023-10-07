using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTestIndexRange
{
    public partial class HisTestIndexRangeDAO : EntityBase
    {
        public List<V_HIS_TEST_INDEX_RANGE> GetView(HisTestIndexRangeSO search, CommonParam param)
        {
            List<V_HIS_TEST_INDEX_RANGE> result = new List<V_HIS_TEST_INDEX_RANGE>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_TEST_INDEX_RANGE GetViewById(long id, HisTestIndexRangeSO search)
        {
            V_HIS_TEST_INDEX_RANGE result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
