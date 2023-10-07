using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAcinInteractive
{
    public partial class HisAcinInteractiveDAO : EntityBase
    {
        private HisAcinInteractiveGet GetWorker
        {
            get
            {
                return (HisAcinInteractiveGet)Worker.Get<HisAcinInteractiveGet>();
            }
        }
        public List<HIS_ACIN_INTERACTIVE> Get(HisAcinInteractiveSO search, CommonParam param)
        {
            List<HIS_ACIN_INTERACTIVE> result = new List<HIS_ACIN_INTERACTIVE>();
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

        public HIS_ACIN_INTERACTIVE GetById(long id, HisAcinInteractiveSO search)
        {
            HIS_ACIN_INTERACTIVE result = null;
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
