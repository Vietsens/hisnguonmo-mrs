using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBranch
{
    public partial class HisBranchDAO : EntityBase
    {
        private HisBranchGet GetWorker
        {
            get
            {
                return (HisBranchGet)Worker.Get<HisBranchGet>();
            }
        }
        public List<HIS_BRANCH> Get(HisBranchSO search, CommonParam param)
        {
            List<HIS_BRANCH> result = new List<HIS_BRANCH>();
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

        public HIS_BRANCH GetById(long id, HisBranchSO search)
        {
            HIS_BRANCH result = null;
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
