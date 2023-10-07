using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServBill
{
    public partial class HisSereServBillDAO : EntityBase
    {
        private HisSereServBillGet GetWorker
        {
            get
            {
                return (HisSereServBillGet)Worker.Get<HisSereServBillGet>();
            }
        }
        public List<HIS_SERE_SERV_BILL> Get(HisSereServBillSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_BILL> result = new List<HIS_SERE_SERV_BILL>();
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

        public HIS_SERE_SERV_BILL GetById(long id, HisSereServBillSO search)
        {
            HIS_SERE_SERV_BILL result = null;
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
