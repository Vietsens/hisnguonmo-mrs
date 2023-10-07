using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAnticipateMaty
{
    public partial class HisAnticipateMatyDAO : EntityBase
    {
        private HisAnticipateMatyGet GetWorker
        {
            get
            {
                return (HisAnticipateMatyGet)Worker.Get<HisAnticipateMatyGet>();
            }
        }
        public List<HIS_ANTICIPATE_MATY> Get(HisAnticipateMatySO search, CommonParam param)
        {
            List<HIS_ANTICIPATE_MATY> result = new List<HIS_ANTICIPATE_MATY>();
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

        public HIS_ANTICIPATE_MATY GetById(long id, HisAnticipateMatySO search)
        {
            HIS_ANTICIPATE_MATY result = null;
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
