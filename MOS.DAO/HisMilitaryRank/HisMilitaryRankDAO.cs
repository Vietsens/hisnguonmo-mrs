using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMilitaryRank
{
    public partial class HisMilitaryRankDAO : EntityBase
    {
        private HisMilitaryRankGet GetWorker
        {
            get
            {
                return (HisMilitaryRankGet)Worker.Get<HisMilitaryRankGet>();
            }
        }
        public List<HIS_MILITARY_RANK> Get(HisMilitaryRankSO search, CommonParam param)
        {
            List<HIS_MILITARY_RANK> result = new List<HIS_MILITARY_RANK>();
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

        public HIS_MILITARY_RANK GetById(long id, HisMilitaryRankSO search)
        {
            HIS_MILITARY_RANK result = null;
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
