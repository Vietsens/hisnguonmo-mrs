using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentBodyPart
{
    public partial class HisAccidentBodyPartDAO : EntityBase
    {
        public HIS_ACCIDENT_BODY_PART GetByCode(string code, HisAccidentBodyPartSO search)
        {
            HIS_ACCIDENT_BODY_PART result = null;

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

        public Dictionary<string, HIS_ACCIDENT_BODY_PART> GetDicByCode(HisAccidentBodyPartSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_BODY_PART> result = new Dictionary<string, HIS_ACCIDENT_BODY_PART>();
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
