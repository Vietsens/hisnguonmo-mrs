using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServDebt
{
    public partial class HisSereServDebtDAO : EntityBase
    {
        private HisSereServDebtGet GetWorker
        {
            get
            {
                return (HisSereServDebtGet)Worker.Get<HisSereServDebtGet>();
            }
        }
        public List<HIS_SERE_SERV_DEBT> Get(HisSereServDebtSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_DEBT> result = new List<HIS_SERE_SERV_DEBT>();
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

        public HIS_SERE_SERV_DEBT GetById(long id, HisSereServDebtSO search)
        {
            HIS_SERE_SERV_DEBT result = null;
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
