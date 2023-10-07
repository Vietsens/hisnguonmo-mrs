using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAnticipateMety
{
    public partial class HisAnticipateMetyDAO : EntityBase
    {
        private HisAnticipateMetyGet GetWorker
        {
            get
            {
                return (HisAnticipateMetyGet)Worker.Get<HisAnticipateMetyGet>();
            }
        }
        public List<HIS_ANTICIPATE_METY> Get(HisAnticipateMetySO search, CommonParam param)
        {
            List<HIS_ANTICIPATE_METY> result = new List<HIS_ANTICIPATE_METY>();
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

        public HIS_ANTICIPATE_METY GetById(long id, HisAnticipateMetySO search)
        {
            HIS_ANTICIPATE_METY result = null;
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
