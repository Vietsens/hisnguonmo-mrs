using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttCatastrophe
{
    public partial class HisPtttCatastropheDAO : EntityBase
    {
        private HisPtttCatastropheGet GetWorker
        {
            get
            {
                return (HisPtttCatastropheGet)Worker.Get<HisPtttCatastropheGet>();
            }
        }
        public List<HIS_PTTT_CATASTROPHE> Get(HisPtttCatastropheSO search, CommonParam param)
        {
            List<HIS_PTTT_CATASTROPHE> result = new List<HIS_PTTT_CATASTROPHE>();
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

        public HIS_PTTT_CATASTROPHE GetById(long id, HisPtttCatastropheSO search)
        {
            HIS_PTTT_CATASTROPHE result = null;
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
