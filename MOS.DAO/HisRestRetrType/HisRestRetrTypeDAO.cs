using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRestRetrType
{
    public partial class HisRestRetrTypeDAO : EntityBase
    {
        private HisRestRetrTypeGet GetWorker
        {
            get
            {
                return (HisRestRetrTypeGet)Worker.Get<HisRestRetrTypeGet>();
            }
        }
        public List<HIS_REST_RETR_TYPE> Get(HisRestRetrTypeSO search, CommonParam param)
        {
            List<HIS_REST_RETR_TYPE> result = new List<HIS_REST_RETR_TYPE>();
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

        public HIS_REST_RETR_TYPE GetById(long id, HisRestRetrTypeSO search)
        {
            HIS_REST_RETR_TYPE result = null;
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
