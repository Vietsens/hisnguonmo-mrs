using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEkipUser
{
    public partial class HisEkipUserDAO : EntityBase
    {
        private HisEkipUserGet GetWorker
        {
            get
            {
                return (HisEkipUserGet)Worker.Get<HisEkipUserGet>();
            }
        }
        public List<HIS_EKIP_USER> Get(HisEkipUserSO search, CommonParam param)
        {
            List<HIS_EKIP_USER> result = new List<HIS_EKIP_USER>();
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

        public HIS_EKIP_USER GetById(long id, HisEkipUserSO search)
        {
            HIS_EKIP_USER result = null;
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
