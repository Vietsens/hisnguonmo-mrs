using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPayForm
{
    public partial class HisPayFormDAO : EntityBase
    {
        public HIS_PAY_FORM GetByCode(string code, HisPayFormSO search)
        {
            HIS_PAY_FORM result = null;

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

        public Dictionary<string, HIS_PAY_FORM> GetDicByCode(HisPayFormSO search, CommonParam param)
        {
            Dictionary<string, HIS_PAY_FORM> result = new Dictionary<string, HIS_PAY_FORM>();
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
