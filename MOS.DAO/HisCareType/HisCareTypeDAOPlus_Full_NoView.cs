using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareType
{
    public partial class HisCareTypeDAO : EntityBase
    {
        public HIS_CARE_TYPE GetByCode(string code, HisCareTypeSO search)
        {
            HIS_CARE_TYPE result = null;

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

        public Dictionary<string, HIS_CARE_TYPE> GetDicByCode(HisCareTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_CARE_TYPE> result = new Dictionary<string, HIS_CARE_TYPE>();
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
