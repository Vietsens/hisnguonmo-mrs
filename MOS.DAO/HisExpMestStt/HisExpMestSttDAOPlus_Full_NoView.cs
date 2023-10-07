using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestStt
{
    public partial class HisExpMestSttDAO : EntityBase
    {
        public HIS_EXP_MEST_STT GetByCode(string code, HisExpMestSttSO search)
        {
            HIS_EXP_MEST_STT result = null;

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

        public Dictionary<string, HIS_EXP_MEST_STT> GetDicByCode(HisExpMestSttSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXP_MEST_STT> result = new Dictionary<string, HIS_EXP_MEST_STT>();
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
