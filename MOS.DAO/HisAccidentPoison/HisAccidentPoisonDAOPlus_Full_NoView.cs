using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentPoison
{
    public partial class HisAccidentPoisonDAO : EntityBase
    {
        public HIS_ACCIDENT_POISON GetByCode(string code, HisAccidentPoisonSO search)
        {
            HIS_ACCIDENT_POISON result = null;

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

        public Dictionary<string, HIS_ACCIDENT_POISON> GetDicByCode(HisAccidentPoisonSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_POISON> result = new Dictionary<string, HIS_ACCIDENT_POISON>();
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
