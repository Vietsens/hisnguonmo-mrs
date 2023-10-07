using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServDeposit
{
    public partial class HisSereServDepositDAO : EntityBase
    {
        private HisSereServDepositGet GetWorker
        {
            get
            {
                return (HisSereServDepositGet)Worker.Get<HisSereServDepositGet>();
            }
        }
        public List<HIS_SERE_SERV_DEPOSIT> Get(HisSereServDepositSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_DEPOSIT> result = new List<HIS_SERE_SERV_DEPOSIT>();
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

        public HIS_SERE_SERV_DEPOSIT GetById(long id, HisSereServDepositSO search)
        {
            HIS_SERE_SERV_DEPOSIT result = null;
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
