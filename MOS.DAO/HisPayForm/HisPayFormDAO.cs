using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPayForm
{
    public partial class HisPayFormDAO : EntityBase
    {
        private HisPayFormGet GetWorker
        {
            get
            {
                return (HisPayFormGet)Worker.Get<HisPayFormGet>();
            }
        }
        public List<HIS_PAY_FORM> Get(HisPayFormSO search, CommonParam param)
        {
            List<HIS_PAY_FORM> result = new List<HIS_PAY_FORM>();
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

        public HIS_PAY_FORM GetById(long id, HisPayFormSO search)
        {
            HIS_PAY_FORM result = null;
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
