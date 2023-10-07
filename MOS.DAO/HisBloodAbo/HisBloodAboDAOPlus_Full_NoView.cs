using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodAbo
{
    public partial class HisBloodAboDAO : EntityBase
    {
        public HIS_BLOOD_ABO GetByCode(string code, HisBloodAboSO search)
        {
            HIS_BLOOD_ABO result = null;

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

        public Dictionary<string, HIS_BLOOD_ABO> GetDicByCode(HisBloodAboSO search, CommonParam param)
        {
            Dictionary<string, HIS_BLOOD_ABO> result = new Dictionary<string, HIS_BLOOD_ABO>();
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
