using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPaanPosition
{
    public partial class HisPaanPositionDAO : EntityBase
    {
        private HisPaanPositionGet GetWorker
        {
            get
            {
                return (HisPaanPositionGet)Worker.Get<HisPaanPositionGet>();
            }
        }
        public List<HIS_PAAN_POSITION> Get(HisPaanPositionSO search, CommonParam param)
        {
            List<HIS_PAAN_POSITION> result = new List<HIS_PAAN_POSITION>();
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

        public HIS_PAAN_POSITION GetById(long id, HisPaanPositionSO search)
        {
            HIS_PAAN_POSITION result = null;
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
