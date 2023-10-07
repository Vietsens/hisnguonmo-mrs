using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEkipTemp
{
    public partial class HisEkipTempDAO : EntityBase
    {
        private HisEkipTempGet GetWorker
        {
            get
            {
                return (HisEkipTempGet)Worker.Get<HisEkipTempGet>();
            }
        }

        public List<HIS_EKIP_TEMP> Get(HisEkipTempSO search, CommonParam param)
        {
            List<HIS_EKIP_TEMP> result = new List<HIS_EKIP_TEMP>();
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

        public HIS_EKIP_TEMP GetById(long id, HisEkipTempSO search)
        {
            HIS_EKIP_TEMP result = null;
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
