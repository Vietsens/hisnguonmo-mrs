using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccountBook
{
    public partial class HisAccountBookDAO : EntityBase
    {
        private HisAccountBookGet GetWorker
        {
            get
            {
                return (HisAccountBookGet)Worker.Get<HisAccountBookGet>();
            }
        }
        public List<HIS_ACCOUNT_BOOK> Get(HisAccountBookSO search, CommonParam param)
        {
            List<HIS_ACCOUNT_BOOK> result = new List<HIS_ACCOUNT_BOOK>();
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

        public HIS_ACCOUNT_BOOK GetById(long id, HisAccountBookSO search)
        {
            HIS_ACCOUNT_BOOK result = null;
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
