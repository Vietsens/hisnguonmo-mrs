using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestUser
{
    public partial class HisImpMestUserDAO : EntityBase
    {
        private HisImpMestUserGet GetWorker
        {
            get
            {
                return (HisImpMestUserGet)Worker.Get<HisImpMestUserGet>();
            }
        }
        public List<HIS_IMP_MEST_USER> Get(HisImpMestUserSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_USER> result = new List<HIS_IMP_MEST_USER>();
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

        public HIS_IMP_MEST_USER GetById(long id, HisImpMestUserSO search)
        {
            HIS_IMP_MEST_USER result = null;
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
