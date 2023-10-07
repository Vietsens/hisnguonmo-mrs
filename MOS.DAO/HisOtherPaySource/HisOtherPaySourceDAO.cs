using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisOtherPaySource
{
    public partial class HisOtherPaySourceDAO : EntityBase
    {
       
        private HisOtherPaySourceGet GetWorker
        {
            get
            {
                return (HisOtherPaySourceGet)Worker.Get<HisOtherPaySourceGet>();
            }
        }

        public List<HIS_OTHER_PAY_SOURCE> Get(HisOtherPaySourceSO search, CommonParam param)
        {
            List<HIS_OTHER_PAY_SOURCE> result = new List<HIS_OTHER_PAY_SOURCE>();
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

        public HIS_OTHER_PAY_SOURCE GetById(long id, HisOtherPaySourceSO search)
        {
            HIS_OTHER_PAY_SOURCE result = null;
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
