using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttTable
{
    public partial class HisPtttTableDAO : EntityBase
    {
        private HisPtttTableGet GetWorker
        {
            get
            {
                return (HisPtttTableGet)Worker.Get<HisPtttTableGet>();
            }
        }
        public List<HIS_PTTT_TABLE> Get(HisPtttTableSO search, CommonParam param)
        {
            List<HIS_PTTT_TABLE> result = new List<HIS_PTTT_TABLE>();
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

        public HIS_PTTT_TABLE GetById(long id, HisPtttTableSO search)
        {
            HIS_PTTT_TABLE result = null;
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
