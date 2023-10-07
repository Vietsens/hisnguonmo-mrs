using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEkipTempUser
{
    public partial class HisEkipTempUserDAO : EntityBase
    {
        private HisEkipTempUserGet GetWorker
        {
            get
            {
                return (HisEkipTempUserGet)Worker.Get<HisEkipTempUserGet>();
            }
        }

        public List<HIS_EKIP_TEMP_USER> Get(HisEkipTempUserSO search, CommonParam param)
        {
            List<HIS_EKIP_TEMP_USER> result = new List<HIS_EKIP_TEMP_USER>();
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

        public HIS_EKIP_TEMP_USER GetById(long id, HisEkipTempUserSO search)
        {
            HIS_EKIP_TEMP_USER result = null;
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
