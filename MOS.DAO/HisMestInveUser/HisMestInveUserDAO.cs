using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestInveUser
{
    public partial class HisMestInveUserDAO : EntityBase
    {
        private HisMestInveUserGet GetWorker
        {
            get
            {
                return (HisMestInveUserGet)Worker.Get<HisMestInveUserGet>();
            }
        }
        public List<HIS_MEST_INVE_USER> Get(HisMestInveUserSO search, CommonParam param)
        {
            List<HIS_MEST_INVE_USER> result = new List<HIS_MEST_INVE_USER>();
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

        public HIS_MEST_INVE_USER GetById(long id, HisMestInveUserSO search)
        {
            HIS_MEST_INVE_USER result = null;
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
