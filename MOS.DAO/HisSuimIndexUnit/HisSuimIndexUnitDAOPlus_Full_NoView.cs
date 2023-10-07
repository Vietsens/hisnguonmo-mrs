using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSuimIndexUnit
{
    public partial class HisSuimIndexUnitDAO : EntityBase
    {
        public HIS_SUIM_INDEX_UNIT GetByCode(string code, HisSuimIndexUnitSO search)
        {
            HIS_SUIM_INDEX_UNIT result = null;

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

        public Dictionary<string, HIS_SUIM_INDEX_UNIT> GetDicByCode(HisSuimIndexUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_SUIM_INDEX_UNIT> result = new Dictionary<string, HIS_SUIM_INDEX_UNIT>();
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
