using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestMetyDepa
{
    public partial class HisMestMetyDepaDAO : EntityBase
    {
        private HisMestMetyDepaGet GetWorker
        {
            get
            {
                return (HisMestMetyDepaGet)Worker.Get<HisMestMetyDepaGet>();
            }
        }
        public List<HIS_MEST_METY_DEPA> Get(HisMestMetyDepaSO search, CommonParam param)
        {
            List<HIS_MEST_METY_DEPA> result = new List<HIS_MEST_METY_DEPA>();
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

        public HIS_MEST_METY_DEPA GetById(long id, HisMestMetyDepaSO search)
        {
            HIS_MEST_METY_DEPA result = null;
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
