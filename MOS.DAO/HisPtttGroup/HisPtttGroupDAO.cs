using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttGroup
{
    public partial class HisPtttGroupDAO : EntityBase
    {
        private HisPtttGroupGet GetWorker
        {
            get
            {
                return (HisPtttGroupGet)Worker.Get<HisPtttGroupGet>();
            }
        }
        public List<HIS_PTTT_GROUP> Get(HisPtttGroupSO search, CommonParam param)
        {
            List<HIS_PTTT_GROUP> result = new List<HIS_PTTT_GROUP>();
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

        public HIS_PTTT_GROUP GetById(long id, HisPtttGroupSO search)
        {
            HIS_PTTT_GROUP result = null;
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
