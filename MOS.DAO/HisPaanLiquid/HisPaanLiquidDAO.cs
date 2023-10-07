using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPaanLiquid
{
    public partial class HisPaanLiquidDAO : EntityBase
    {
        private HisPaanLiquidGet GetWorker
        {
            get
            {
                return (HisPaanLiquidGet)Worker.Get<HisPaanLiquidGet>();
            }
        }
        public List<HIS_PAAN_LIQUID> Get(HisPaanLiquidSO search, CommonParam param)
        {
            List<HIS_PAAN_LIQUID> result = new List<HIS_PAAN_LIQUID>();
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

        public HIS_PAAN_LIQUID GetById(long id, HisPaanLiquidSO search)
        {
            HIS_PAAN_LIQUID result = null;
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
