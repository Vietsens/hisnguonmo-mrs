using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTestIndex
{
    public partial class HisTestIndexDAO : EntityBase
    {
        private HisTestIndexGet GetWorker
        {
            get
            {
                return (HisTestIndexGet)Worker.Get<HisTestIndexGet>();
            }
        }
        public List<HIS_TEST_INDEX> Get(HisTestIndexSO search, CommonParam param)
        {
            List<HIS_TEST_INDEX> result = new List<HIS_TEST_INDEX>();
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

        public HIS_TEST_INDEX GetById(long id, HisTestIndexSO search)
        {
            HIS_TEST_INDEX result = null;
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
