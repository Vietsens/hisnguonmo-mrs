using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentCare
{
    public partial class HisAccidentCareDAO : EntityBase
    {
        public HIS_ACCIDENT_CARE GetByCode(string code, HisAccidentCareSO search)
        {
            HIS_ACCIDENT_CARE result = null;

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

        public Dictionary<string, HIS_ACCIDENT_CARE> GetDicByCode(HisAccidentCareSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_CARE> result = new Dictionary<string, HIS_ACCIDENT_CARE>();
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
