using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisGender
{
    public partial class HisGenderDAO : EntityBase
    {
        public HIS_GENDER GetByCode(string code, HisGenderSO search)
        {
            HIS_GENDER result = null;

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

        public Dictionary<string, HIS_GENDER> GetDicByCode(HisGenderSO search, CommonParam param)
        {
            Dictionary<string, HIS_GENDER> result = new Dictionary<string, HIS_GENDER>();
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
