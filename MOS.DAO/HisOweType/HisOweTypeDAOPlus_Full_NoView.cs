using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisOweType
{
    public partial class HisOweTypeDAO : EntityBase
    {
        public HIS_OWE_TYPE GetByCode(string code, HisOweTypeSO search)
        {
            HIS_OWE_TYPE result = null;

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

        public Dictionary<string, HIS_OWE_TYPE> GetDicByCode(HisOweTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_OWE_TYPE> result = new Dictionary<string, HIS_OWE_TYPE>();
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
