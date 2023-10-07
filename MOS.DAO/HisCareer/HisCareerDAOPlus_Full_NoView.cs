using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareer
{
    public partial class HisCareerDAO : EntityBase
    {
        public HIS_CAREER GetByCode(string code, HisCareerSO search)
        {
            HIS_CAREER result = null;

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

        public Dictionary<string, HIS_CAREER> GetDicByCode(HisCareerSO search, CommonParam param)
        {
            Dictionary<string, HIS_CAREER> result = new Dictionary<string, HIS_CAREER>();
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
