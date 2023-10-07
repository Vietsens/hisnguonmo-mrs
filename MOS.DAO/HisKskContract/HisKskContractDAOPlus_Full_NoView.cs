using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisKskContract
{
    public partial class HisKskContractDAO : EntityBase
    {
        public HIS_KSK_CONTRACT GetByCode(string code, HisKskContractSO search)
        {
            HIS_KSK_CONTRACT result = null;

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

        public Dictionary<string, HIS_KSK_CONTRACT> GetDicByCode(HisKskContractSO search, CommonParam param)
        {
            Dictionary<string, HIS_KSK_CONTRACT> result = new Dictionary<string, HIS_KSK_CONTRACT>();
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
