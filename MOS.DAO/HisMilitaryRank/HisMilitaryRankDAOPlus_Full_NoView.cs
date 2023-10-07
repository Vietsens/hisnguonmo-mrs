using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMilitaryRank
{
    public partial class HisMilitaryRankDAO : EntityBase
    {
        public HIS_MILITARY_RANK GetByCode(string code, HisMilitaryRankSO search)
        {
            HIS_MILITARY_RANK result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_MILITARY_RANK> GetDicByCode(HisMilitaryRankSO search, CommonParam param)
        {
            Dictionary<string, HIS_MILITARY_RANK> result = new Dictionary<string, HIS_MILITARY_RANK>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
