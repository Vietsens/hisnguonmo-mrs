using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestUser
{
    public partial class HisExpMestUserDAO : EntityBase
    {
        private HisExpMestUserGet GetWorker
        {
            get
            {
                return (HisExpMestUserGet)Worker.Get<HisExpMestUserGet>();
            }
        }
        public List<HIS_EXP_MEST_USER> Get(HisExpMestUserSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_USER> result = new List<HIS_EXP_MEST_USER>();
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

        public HIS_EXP_MEST_USER GetById(long id, HisExpMestUserSO search)
        {
            HIS_EXP_MEST_USER result = null;
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
