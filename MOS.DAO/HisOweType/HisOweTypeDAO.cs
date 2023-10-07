using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisOweType
{
    public partial class HisOweTypeDAO : EntityBase
    {
        private HisOweTypeGet GetWorker
        {
            get
            {
                return (HisOweTypeGet)Worker.Get<HisOweTypeGet>();
            }
        }
        public List<HIS_OWE_TYPE> Get(HisOweTypeSO search, CommonParam param)
        {
            List<HIS_OWE_TYPE> result = new List<HIS_OWE_TYPE>();
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

        public HIS_OWE_TYPE GetById(long id, HisOweTypeSO search)
        {
            HIS_OWE_TYPE result = null;
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
