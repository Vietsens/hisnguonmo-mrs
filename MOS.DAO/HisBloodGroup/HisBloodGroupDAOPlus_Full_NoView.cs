using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodGroup
{
    public partial class HisBloodGroupDAO : EntityBase
    {
        public HIS_BLOOD_GROUP GetByCode(string code, HisBloodGroupSO search)
        {
            HIS_BLOOD_GROUP result = null;

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

        public Dictionary<string, HIS_BLOOD_GROUP> GetDicByCode(HisBloodGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_BLOOD_GROUP> result = new Dictionary<string, HIS_BLOOD_GROUP>();
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
