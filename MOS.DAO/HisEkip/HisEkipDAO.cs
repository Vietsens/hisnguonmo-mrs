using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEkip
{
    public partial class HisEkipDAO : EntityBase
    {
        private HisEkipGet GetWorker
        {
            get
            {
                return (HisEkipGet)Worker.Get<HisEkipGet>();
            }
        }
        public List<HIS_EKIP> Get(HisEkipSO search, CommonParam param)
        {
            List<HIS_EKIP> result = new List<HIS_EKIP>();
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

        public HIS_EKIP GetById(long id, HisEkipSO search)
        {
            HIS_EKIP result = null;
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
