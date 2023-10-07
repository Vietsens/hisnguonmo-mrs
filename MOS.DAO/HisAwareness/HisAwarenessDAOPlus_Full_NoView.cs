using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAwareness
{
    public partial class HisAwarenessDAO : EntityBase
    {
        public HIS_AWARENESS GetByCode(string code, HisAwarenessSO search)
        {
            HIS_AWARENESS result = null;

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

        public Dictionary<string, HIS_AWARENESS> GetDicByCode(HisAwarenessSO search, CommonParam param)
        {
            Dictionary<string, HIS_AWARENESS> result = new Dictionary<string, HIS_AWARENESS>();
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
