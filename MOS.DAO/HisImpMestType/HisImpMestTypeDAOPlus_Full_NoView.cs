using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestType
{
    public partial class HisImpMestTypeDAO : EntityBase
    {
        public HIS_IMP_MEST_TYPE GetByCode(string code, HisImpMestTypeSO search)
        {
            HIS_IMP_MEST_TYPE result = null;

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

        public Dictionary<string, HIS_IMP_MEST_TYPE> GetDicByCode(HisImpMestTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_IMP_MEST_TYPE> result = new Dictionary<string, HIS_IMP_MEST_TYPE>();
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
