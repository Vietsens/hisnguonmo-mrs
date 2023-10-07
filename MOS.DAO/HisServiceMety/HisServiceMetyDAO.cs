using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceMety
{
    public partial class HisServiceMetyDAO : EntityBase
    {
        private HisServiceMetyGet GetWorker
        {
            get
            {
                return (HisServiceMetyGet)Worker.Get<HisServiceMetyGet>();
            }
        }
        public List<HIS_SERVICE_METY> Get(HisServiceMetySO search, CommonParam param)
        {
            List<HIS_SERVICE_METY> result = new List<HIS_SERVICE_METY>();
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

        public HIS_SERVICE_METY GetById(long id, HisServiceMetySO search)
        {
            HIS_SERVICE_METY result = null;
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
