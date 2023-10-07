using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServSegr
{
    public partial class HisServSegrDAO : EntityBase
    {
        private HisServSegrGet GetWorker
        {
            get
            {
                return (HisServSegrGet)Worker.Get<HisServSegrGet>();
            }
        }
        public List<HIS_SERV_SEGR> Get(HisServSegrSO search, CommonParam param)
        {
            List<HIS_SERV_SEGR> result = new List<HIS_SERV_SEGR>();
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

        public HIS_SERV_SEGR GetById(long id, HisServSegrSO search)
        {
            HIS_SERV_SEGR result = null;
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
