using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisKskContract
{
    public partial class HisKskContractDAO : EntityBase
    {
        private HisKskContractGet GetWorker
        {
            get
            {
                return (HisKskContractGet)Worker.Get<HisKskContractGet>();
            }
        }
        public List<HIS_KSK_CONTRACT> Get(HisKskContractSO search, CommonParam param)
        {
            List<HIS_KSK_CONTRACT> result = new List<HIS_KSK_CONTRACT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_KSK_CONTRACT GetById(long id, HisKskContractSO search)
        {
            HIS_KSK_CONTRACT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
