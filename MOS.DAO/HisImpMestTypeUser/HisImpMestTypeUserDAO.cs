using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestTypeUser
{
    public partial class HisImpMestTypeUserDAO : EntityBase
    {
        private HisImpMestTypeUserGet GetWorker
        {
            get
            {
                return (HisImpMestTypeUserGet)Worker.Get<HisImpMestTypeUserGet>();
            }
        }
        public List<HIS_IMP_MEST_TYPE_USER> Get(HisImpMestTypeUserSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_TYPE_USER> result = new List<HIS_IMP_MEST_TYPE_USER>();
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

        public HIS_IMP_MEST_TYPE_USER GetById(long id, HisImpMestTypeUserSO search)
        {
            HIS_IMP_MEST_TYPE_USER result = null;
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
