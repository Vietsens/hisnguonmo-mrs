using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestStt
{
    public partial class HisImpMestSttDAO : EntityBase
    {
        public HIS_IMP_MEST_STT GetByCode(string code, HisImpMestSttSO search)
        {
            HIS_IMP_MEST_STT result = null;

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

        public Dictionary<string, HIS_IMP_MEST_STT> GetDicByCode(HisImpMestSttSO search, CommonParam param)
        {
            Dictionary<string, HIS_IMP_MEST_STT> result = new Dictionary<string, HIS_IMP_MEST_STT>();
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
