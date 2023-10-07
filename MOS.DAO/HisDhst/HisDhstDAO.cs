using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDhst
{
    public partial class HisDhstDAO : EntityBase
    {
        private HisDhstGet GetWorker
        {
            get
            {
                return (HisDhstGet)Worker.Get<HisDhstGet>();
            }
        }
        public List<HIS_DHST> Get(HisDhstSO search, CommonParam param)
        {
            List<HIS_DHST> result = new List<HIS_DHST>();
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

        public HIS_DHST GetById(long id, HisDhstSO search)
        {
            HIS_DHST result = null;
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
